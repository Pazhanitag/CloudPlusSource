using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Enums;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Subscription;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity;

namespace CloudPlus.AppServices.Office365.Workflow.ManageSubscription
{
    public class ManageSubscriptionWorkflow : IManageSubscriptionWorkflow
    {
        private readonly IUserService _userService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IActivityConfigurator _activityConfigurator;

        public ManageSubscriptionWorkflow(
            IUserService userService,
            IOffice365DbCustomerService office365DbCustomerService,
            IOffice365DbUserService office365DbUserService,
            IOffice365DbSubscriptionService office365DbSubscriptionService,
            IActivityConfigurator activityConfigurator)
        {
            _userService = userService;
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbUserService = office365DbUserService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IManageSubscriptionsAndLicencesCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var message = context.Message;
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(message.CompanyId);
            var office365Users =
                await _office365DbUserService.GetAllCustomerUsersWithLicensesAndOfferAsync(office365Customer.Id);
            var subscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                office365Customer.Office365CustomerId, message.CloudPlusProductIdentifier);
            var newSubscriptionChangeCount = 0;

            var usersPrincipalName = new List<string>();
            foreach (var user in message.Users)
            {
                usersPrincipalName.Add(user.UserPrincipalName);
            }

            builder.AddVariable("userPrincipalName", usersPrincipalName);
            builder.AddVariable("workflowActivityType", WorkflowActivityType.Office365ManageSubscription.ToString());

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.Faulted |
                RoutingSlipEvents.Completed |
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.HardDeleteUser)
            {
                var office365User = office365Users.FirstOrDefault(u =>
                    u.UserPrincipalName == message.Users.FirstOrDefault().UserPrincipalName);

                var subscriptionToRemove =
                    await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                        office365Customer.Office365CustomerId,
                        office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);


                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.OperationInProgress,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                if (subscriptionToRemove.Quantity == 1)
                {
                    builder.AddActivity("SuspendPartnerPlatformSubscription",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                        {
                            subscriptionToRemove.Office365CustomerId,
                            subscriptionToRemove.Office365SubscriptionId
                        });

                    builder.AddActivity("SuspendDatabaseSubscription",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(ISuspendDatabasesubscriptionActivity)), new
                        {
                            subscriptionToRemove.Office365SubscriptionId
                        });
                }
                else
                {
                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = -1,
                            subscriptionToRemove.Office365SubscriptionId
                        });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = -1,
                            subscriptionToRemove.Office365CustomerId,
                            subscriptionToRemove.Office365SubscriptionId
                        });
                }

                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365HardDeleteUserUri,
                    RoutingSlipEvents.Completed,
                    x => x.Send<IOffice365HardDeleteUserCommand>(new
                    {
                        office365Customer.Office365CustomerId,
                        message.Users.FirstOrDefault().UserPrincipalName
                    }));

                var routingSlip = builder.Build();

                await context.Execute(routingSlip);
            }
            else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.RemoveLicence)
            {
                var office365User = office365Users.FirstOrDefault(u =>
                    u.UserPrincipalName == message.Users.FirstOrDefault().UserPrincipalName);

                var subscriptionToRemove =
                    await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                        office365Customer.Office365CustomerId,
                        office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);


                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.OperationInProgress,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                if (subscriptionToRemove.Quantity == 1)
                {
                    builder.AddActivity("SuspendPartnerPlatformSubscription",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                        {
                            subscriptionToRemove.Office365CustomerId,
                            subscriptionToRemove.Office365SubscriptionId
                        });

                    builder.AddActivity("SuspendDatabaseSubscription",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(ISuspendDatabasesubscriptionActivity)), new
                        {
                            subscriptionToRemove.Office365SubscriptionId
                        });
                }
                else
                {
                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = -1,
                            subscriptionToRemove.Office365SubscriptionId
                        });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = -1,
                            subscriptionToRemove.Office365CustomerId,
                            subscriptionToRemove.Office365SubscriptionId
                        });
                }

                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserRemoveLicenseUri,
                    RoutingSlipEvents.Completed,
                    x => x.Send<IOffice365UserRemoveLicenseCommand>(new
                    {
                        message.CompanyId,
                        message.Users.FirstOrDefault().UserPrincipalName
                    }));

                var routingSlip = builder.Build();

                await context.Execute(routingSlip);
            }
            else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.RestoreUser)
            {
                var office365User = office365Users.FirstOrDefault(u =>
                    u.UserPrincipalName == message.Users.FirstOrDefault().UserPrincipalName);

                var subscriptionToRestore =
                    await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                        office365Customer.Office365CustomerId,
                        office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);


                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.OperationInProgress,
                        subscriptionToRestore.Office365SubscriptionId
                    });

                if (subscriptionToRestore.Suspended)
                {
                    builder.AddActivity("ActivateSuspendedDatabaseSubscriptionActivity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IActivateSuspendedDatabaseSubscriptionActivity)), new
                        {
                            subscriptionToRestore.Office365SubscriptionId
                        });

                    builder.AddActivity("ActivateSuspendedPartnerPlatformSubscriptionAcivity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IActivateSuspendedPartnerPlatformSubscriptionAcivity)), new
                        {
                            subscriptionToRestore.Office365CustomerId,
                            subscriptionToRestore.Office365SubscriptionId
                        });

                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = 0, // well there is always 1 subscription left
                            subscriptionToRestore.Office365SubscriptionId
                        });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = 0, // well there is always 1 subscription left
                            subscriptionToRestore.Office365CustomerId,
                            subscriptionToRestore.Office365SubscriptionId
                        });
                }
                else
                {
                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = 1,
                            subscriptionToRestore.Office365SubscriptionId
                        });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                        _activityConfigurator.GetActivityExecuteUri(context,
                            typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                        {
                            QuantityChange = 1,
                            subscriptionToRestore.Office365CustomerId,
                            subscriptionToRestore.Office365SubscriptionId
                        });
                }

                builder.AddActivity("UpdateDatabaseSubscriptionState",
                    _activityConfigurator.GetActivityExecuteUri(context,
                        typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                    {
                        SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                        subscriptionToRestore.Office365SubscriptionId
                    });

                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserRestoreUri,
                    RoutingSlipEvents.Completed,
                    x => x.Send<IOffice365UserRestoreCommand>(new
                    {
                        message.CompanyId,
                        message.Users.FirstOrDefault().UserPrincipalName
                    }));

                var routingSlip = builder.Build();

                await context.Execute(routingSlip);
            }
            else
            {
                foreach (var messageUserPrincipalName in message.Users)
                {
                    var o365User = office365Users.FirstOrDefault(u =>
                        u.UserPrincipalName == messageUserPrincipalName.UserPrincipalName);
                    if (o365User == null)
                    {
                        newSubscriptionChangeCount++;
                        continue;
                    }

                    if (!o365User.Licenses.Any())
                    {
                        newSubscriptionChangeCount++;
                        continue;
                    }

                    if (o365User.Licenses.FirstOrDefault(l =>
                            l.Office365Offer.CloudPlusProductIdentifier == message.CloudPlusProductIdentifier) == null)
                    {
                        newSubscriptionChangeCount++;
                    }
                }

                if (newSubscriptionChangeCount > 0)
                {
                    if (subscription == null)
                    {
                        builder.AddActivity("CreateDatabaseSubscription",
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(ICreateDatabaseSubscriptionActivity)), new
                            {
                                office365Customer.Office365CustomerId,
                                message.CloudPlusProductIdentifier,
                                Quantity = newSubscriptionChangeCount
                            });

                        builder.AddActivity("CreateOrder",
                            _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderActivity)), new
                            {
                                message.CloudPlusProductIdentifier,
                                office365Customer.Office365CustomerId,
                                Quantity = newSubscriptionChangeCount
                            });

                        builder.AddActivity("UpdateDatabaseSubscription",
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IUpdateDatabaseSubscriptionActivity)), new
                            {
                                message.CloudPlusProductIdentifier,
                                office365Customer.Office365CustomerId
                            });

                        builder.AddActivity("UpdateDatabaseSubscriptionState",
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.AvailableForOperations
                            });
                    }
                    else
                    {
                        builder.AddActivity("UpdateDatabaseSubscriptionState",
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.OperationInProgress,
                                subscription.Office365SubscriptionId
                            });

                        if (subscription.Suspended)
                        {
                            builder.AddActivity("ActivateSuspendedDatabaseSubscriptionActivity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IActivateSuspendedDatabaseSubscriptionActivity)), new
                                {
                                    subscription.Office365SubscriptionId
                                });

                            builder.AddActivity("ActivateSuspendedPartnerPlatformSubscriptionAcivity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IActivateSuspendedPartnerPlatformSubscriptionAcivity)), new
                                {
                                    subscription.Office365CustomerId,
                                    subscription.Office365SubscriptionId
                                });

                            builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange =
                                        newSubscriptionChangeCount - 1, // well there is always 1 subscription left
                                    subscription.Office365SubscriptionId
                                });

                            builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange =
                                        newSubscriptionChangeCount - 1, // well there is always 1 subscription left
                                    subscription.Office365CustomerId,
                                    subscription.Office365SubscriptionId
                                });
                        }
                        else
                        {
                            builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange = newSubscriptionChangeCount,
                                    subscription.Office365SubscriptionId
                                });

                            builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange = newSubscriptionChangeCount,
                                    subscription.Office365CustomerId,
                                    subscription.Office365SubscriptionId
                                });
                        }

                        builder.AddActivity("UpdateDatabaseSubscriptionState",
                            _activityConfigurator.GetActivityExecuteUri(context,
                                typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                                subscription.Office365SubscriptionId
                            });
                    }

                    if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.AssignNewLicence)
                    {
                        var user = _userService.GetUser(message.Users.FirstOrDefault().UserPrincipalName);

                        await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                            RoutingSlipEvents.Completed,
                            x => x.Send<IOffice365UserAssignLicenseCommand>(new
                            {
                                message.CompanyId,
                                message.CloudPlusProductIdentifier,
                                UserPrincipalName = user.UserName,
                                user.DisplayName,
                                user.FirstName,
                                user.LastName,
                                UsageLocation = "US",
                                user.City,
                                user.Country,
                                user.PhoneNumber,
                                PostalCode = user.ZipCode,
                                user.State,
                                user.StreetAddress,
                                message.Users.FirstOrDefault().Password,
                                message.UserRoles
                            }));
                    }
                    else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.ChangeLicence)
                    {
                        var office365User =
                            await _office365DbUserService.GetOffice365DatabaseUserAsync(message.Users
                                .FirstOrDefault().UserPrincipalName);

                        if (office365User.Licenses != null && office365User.Licenses.Any())
                        {
                            var decreaseSubscription =
                                await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                                    office365Customer.Office365CustomerId,
                                    office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);

                            builder.AddActivity("UpdateDatabaseSubscriptionState",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                                {
                                    SubscriptionState = Office365SubscriptionState.OperationInProgress,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                            if (decreaseSubscription.Quantity == 1)
                            {
                                builder.AddActivity("SuspendPartnerPlatformSubscription",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                                    {
                                        decreaseSubscription.Office365CustomerId,
                                        decreaseSubscription.Office365SubscriptionId
                                    });

                                builder.AddActivity("SuspendDatabaseSubscription",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(ISuspendDatabasesubscriptionActivity)), new
                                    {
                                        decreaseSubscription.Office365SubscriptionId
                                    });
                            }
                            else
                            {
                                builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                                    {
                                        QuantityChange = -1,
                                        decreaseSubscription.Office365SubscriptionId
                                    });

                                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                    {
                                        QuantityChange = -1,
                                        decreaseSubscription.Office365CustomerId,
                                        decreaseSubscription.Office365SubscriptionId
                                    });
                            }

                            builder.AddActivity("UpdateDatabaseSubscriptionState",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                                {
                                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                            await builder.AddSubscription(Office365ServiceConstants.Office365UserChangeLicenseUri,
                                RoutingSlipEvents.Completed,
                                x => x.Send<IOffice365UserChangeLicenseCommand>(new
                                {
                                    message.CompanyId,
                                    office365Customer.Office365CustomerId,
                                    message.CloudPlusProductIdentifier,
                                    office365User.UserPrincipalName,
                                    AssignCloudPlusProductIdentifier = message.CloudPlusProductIdentifier,
                                    RemoveCloudPlusProductIdentifier = office365User.Licenses.FirstOrDefault()
                                        .Office365Offer.CloudPlusProductIdentifier,
                                    message.UserRoles
                                }));
                        }
                        else
                        {
                            var user = _userService.GetUser(message.Users.FirstOrDefault().UserPrincipalName);

                            await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                                RoutingSlipEvents.Completed,
                                x => x.Send<IOffice365UserAssignLicenseCommand>(new
                                {
                                    message.CompanyId,
                                    message.CloudPlusProductIdentifier,
                                    UserPrincipalName = user.UserName,
                                    user.DisplayName,
                                    user.FirstName,
                                    user.LastName,
                                    UsageLocation = "US",
                                    user.City,
                                    user.Country,
                                    user.PhoneNumber,
                                    PostalCode = user.ZipCode,
                                    user.State,
                                    user.StreetAddress,
                                    message.Users.FirstOrDefault().Password,
                                    message.UserRoles
                                }));
                        }
                    }
                    else
                    {
                        var supscriptionsToChange = new Dictionary<string, int>();
                        foreach (var messageUserPrincipalName in message.Users)
                        {
                            var office365User = office365Users.FirstOrDefault(u =>
                                u.UserPrincipalName == messageUserPrincipalName.UserPrincipalName);

                            if (office365User != null)
                            {
                                if (office365User.Licenses.Any())
                                {
                                    if (office365User.Licenses.FirstOrDefault(l =>
                                            l.Office365Offer.CloudPlusProductIdentifier ==
                                            message.CloudPlusProductIdentifier) == null)
                                    {
                                        await builder.AddSubscription(
                                            Office365ServiceConstants.Office365UserChangeLicenseUri,
                                            RoutingSlipEvents.Completed,
                                            x => x.Send<IOffice365UserChangeLicenseCommand>(new
                                            {
                                                message.CompanyId,
                                                office365Customer.Office365CustomerId,
                                                message.CloudPlusProductIdentifier,
                                                office365User.UserPrincipalName,
                                                AssignCloudPlusProductIdentifier = message.CloudPlusProductIdentifier,
                                                RemoveCloudPlusProductIdentifier = office365User.Licenses
                                                    .FirstOrDefault().Office365Offer.CloudPlusProductIdentifier,
                                                message.UserRoles
                                            }));
                                        var decreaseSubscription = await
                                            _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                                                office365Customer.Office365CustomerId,
                                                office365User.Licenses.FirstOrDefault()?.Office365Offer
                                                    .CloudPlusProductIdentifier);

                                        if (supscriptionsToChange.ContainsKey(decreaseSubscription
                                            .Office365SubscriptionId))
                                            supscriptionsToChange[decreaseSubscription.Office365SubscriptionId]++;
                                        else
                                            supscriptionsToChange.Add(decreaseSubscription.Office365SubscriptionId, 1);
                                    }
                                }
                                else
                                {
                                    var user = _userService.GetUser(messageUserPrincipalName.UserPrincipalName);

                                    await builder.AddSubscription(
                                        Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                                        RoutingSlipEvents.Completed,
                                        x => x.Send<IOffice365UserAssignLicenseCommand>(new
                                        {
                                            message.CompanyId,
                                            message.CloudPlusProductIdentifier,
                                            UserPrincipalName = user.UserName,
                                            user.DisplayName,
                                            user.FirstName,
                                            user.LastName,
                                            UsageLocation = "US",
                                            user.City,
                                            user.Country,
                                            user.PhoneNumber,
                                            PostalCode = user.ZipCode,
                                            user.State,
                                            user.StreetAddress,
                                            messageUserPrincipalName.Password,
                                            message.UserRoles
                                        }));
                                }
                            }
                            else
                            {
                                var user = _userService.GetUser(messageUserPrincipalName.UserPrincipalName);

                                await builder.AddSubscription(
                                    Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                                    RoutingSlipEvents.Completed,
                                    x => x.Send<IOffice365UserAssignLicenseCommand>(new
                                    {
                                        message.CompanyId,
                                        message.CloudPlusProductIdentifier,
                                        UserPrincipalName = user.UserName,
                                        user.DisplayName,
                                        user.FirstName,
                                        user.LastName,
                                        UsageLocation = "US",
                                        user.City,
                                        user.Country,
                                        user.PhoneNumber,
                                        PostalCode = user.ZipCode,
                                        user.State,
                                        user.StreetAddress,
                                        messageUserPrincipalName.Password,
                                        message.UserRoles
                                    }));
                            }
                        }

                        foreach (var subscriptionToChange in supscriptionsToChange)
                        {
                            var decreaseSubscription = await
                                _office365DbSubscriptionService.GetSubscriptionAsyc(subscriptionToChange.Key);

                            builder.AddActivity("UpdateDatabaseSubscriptionState",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                                {
                                    SubscriptionState = Office365SubscriptionState.OperationInProgress,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                            if (decreaseSubscription.Quantity == subscriptionToChange.Value)
                            {
                                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                    {
                                        QuantityChange =
                                            (decreaseSubscription.Quantity - 1) *
                                            -1, // well there is always 1 subscription left
                                        decreaseSubscription.Office365CustomerId,
                                        decreaseSubscription.Office365SubscriptionId
                                    });

                                builder.AddActivity("SuspendPartnerPlatformSubscription",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                                    {
                                        decreaseSubscription.Office365CustomerId,
                                        decreaseSubscription.Office365SubscriptionId
                                    });

                                builder.AddActivity("SuspendDatabaseSubscription",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(ISuspendDatabasesubscriptionActivity)), new
                                    {
                                        decreaseSubscription.Office365SubscriptionId
                                    });
                            }
                            else
                            {
                                builder.AddActivity("UpdateDatabaseSubscriptionQuantity",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                                    {
                                        QuantityChange = subscriptionToChange.Value * -1,
                                        decreaseSubscription.Office365SubscriptionId
                                    });

                                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity",
                                    _activityConfigurator.GetActivityExecuteUri(context,
                                        typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                    {
                                        QuantityChange = subscriptionToChange.Value * -1,
                                        decreaseSubscription.Office365CustomerId,
                                        decreaseSubscription.Office365SubscriptionId
                                    });
                            }

                            builder.AddActivity("UpdateDatabaseSubscriptionState",
                                _activityConfigurator.GetActivityExecuteUri(context,
                                    typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                                {
                                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                                    decreaseSubscription.Office365SubscriptionId
                                });
                        }
                    }

                    var routingSlip = builder.Build();

                    await context.Execute(routingSlip);
                }
            }
        }
    }
}
