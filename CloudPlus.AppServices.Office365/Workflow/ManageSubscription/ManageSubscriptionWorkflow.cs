using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
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
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiPartnerPlatformSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiPartnerPlatformSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiDatabasesubscription;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseSecurityGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365GroupMember;
using CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365GroupMember;

namespace CloudPlus.AppServices.Office365.Workflow.ManageSubscription
{
    public class ManageSubscriptionWorkflow : IManageSubscriptionWorkflow
    {
        private readonly IUserService _userService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IActivityConfigurator _activityConfigurator;

        public ManageSubscriptionWorkflow(IUserService userService, IOffice365DbCustomerService office365DbCustomerService, IOffice365DbUserService office365DbUserService, IOffice365DbSubscriptionService office365DbSubscriptionService, IActivityConfigurator activityConfigurator)
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

            var office365Users = await _office365DbUserService.GetAllCustomerUsersWithLicensesAndOfferAsync(office365Customer.Id);
            //TAG
            var subscriptions = await _office365DbSubscriptionService.GetSubscriptionsByProductIdentifiersAsync(
                office365Customer.Office365CustomerId, message.CloudPlusProductIdentifiers);
            //TAG
            var allSubcriptions = new Dictionary<string, int>();
            var userSubcriptions = new Dictionary<string, string>();
            message.CloudPlusProductIdentifiers.ToList().ForEach(x => allSubcriptions.Add(x, 0));
            var newSubscriptionChangeCount = 0;

            if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.RemoveLicence)
            {
                var office365User = office365Users.FirstOrDefault(u =>
                  u.UserPrincipalName == message.Users.FirstOrDefault().UserPrincipalName);

                var subscriptionToRemove = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                    office365Customer.Office365CustomerId,
                    office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);


                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.OperationInProgress,
                    subscriptionToRemove.Office365SubscriptionId
                });

                if (subscriptionToRemove.Quantity == 1)
                {
                    builder.AddActivity("SuspendPartnerPlatformSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                    {
                        subscriptionToRemove.Office365CustomerId,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                    builder.AddActivity("SuspendDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendDatabasesubscriptionActivity)), new
                    {
                        subscriptionToRemove.Office365SubscriptionId
                    });
                }
                else
                {
                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = -1,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = -1,
                        subscriptionToRemove.Office365CustomerId,
                        subscriptionToRemove.Office365SubscriptionId
                    });

                }

                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                    subscriptionToRemove.Office365SubscriptionId
                });

                builder.AddSubscription(Office365ServiceConstants.RoutingSlipEventObserverUri,
                    RoutingSlipEvents.ActivityCompleted |
                    RoutingSlipEvents.ActivityFaulted |
                    RoutingSlipEvents.ActivityCompensated |
                    RoutingSlipEvents.ActivityCompensationFailed);

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

                var subscriptionToRestore = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                    office365Customer.Office365CustomerId,
                    office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);


                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.OperationInProgress,
                    subscriptionToRestore.Office365SubscriptionId
                });

                if (subscriptionToRestore.Suspended)
                {
                    builder.AddActivity("ActivateSuspendedDatabaseSubscriptionActivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateSuspendedDatabaseSubscriptionActivity)), new
                    {
                        subscriptionToRestore.Office365SubscriptionId
                    });

                    builder.AddActivity("ActivateSuspendedPartnerPlatformSubscriptionAcivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateSuspendedPartnerPlatformSubscriptionAcivity)), new
                    {
                        subscriptionToRestore.Office365CustomerId,
                        subscriptionToRestore.Office365SubscriptionId
                    });

                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = 0, // well there is always 1 subscription left
                        subscriptionToRestore.Office365SubscriptionId
                    });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = 0, // well there is always 1 subscription left
                        subscriptionToRestore.Office365CustomerId,
                        subscriptionToRestore.Office365SubscriptionId
                    });
                }
                else
                {

                    builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = 1,
                        subscriptionToRestore.Office365SubscriptionId
                    });

                    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                    {
                        QuantityChange = 1,
                        subscriptionToRestore.Office365CustomerId,
                        subscriptionToRestore.Office365SubscriptionId
                    });

                }

                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                    subscriptionToRestore.Office365SubscriptionId
                });

                builder.AddSubscription(Office365ServiceConstants.RoutingSlipEventObserverUri,
                    RoutingSlipEvents.ActivityCompleted |
                    RoutingSlipEvents.ActivityFaulted |
                    RoutingSlipEvents.ActivityCompensated |
                    RoutingSlipEvents.ActivityCompensationFailed);

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
            //TAG
            else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.MultiAddUser)
            {
                foreach (var messageUserPrincipalName in message.Users)
                {
                    var o365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == messageUserPrincipalName.UserPrincipalName);
                    if (o365User == null || !o365User.Licenses.Any())
                    {
                        if (subscriptions.Count > 0)
                        {
                            subscriptions.ForEach(x => allSubcriptions[x.Office365Offer.CloudPlusProductIdentifier] += 1);
                            userSubcriptions.Add(messageUserPrincipalName.UserPrincipalName, string.Join(",", subscriptions.Select(x => x.Office365Offer.CloudPlusProductIdentifier)));
                        }

                        else
                        {
                            foreach (var Subcrip in allSubcriptions.AsEnumerable())
                            {
                                allSubcriptions[Subcrip.Key] += 1;
                            }
                            userSubcriptions.Add(messageUserPrincipalName.UserPrincipalName, string.Join(",", allSubcriptions));

                        }
                        //allSubcriptions.ToList().ForEach(x => x.Value + 1);
                        continue;
                    }
                    if (o365User.Licenses.Any())
                    {
                        message.CloudPlusProductIdentifiers.Where(c => !o365User.Licenses.Any(l=>l.Office365Offer.CloudPlusProductIdentifier== c)).ToList()
                            .ForEach(x => allSubcriptions[x] += 1);
                        userSubcriptions.Add(messageUserPrincipalName.UserPrincipalName, string.Join(",", message.CloudPlusProductIdentifiers.Where(x => !o365User.Licenses.Any(o => o.Office365Offer.CloudPlusProductIdentifier == x)).ToArray()));
                    }

                }
                if (allSubcriptions.Count > 0 && allSubcriptions.Sum(x => x.Value) > 0)
                {
                    //Add New Subscription 
                    if (subscriptions == null)
                        subscriptions = new List<Models.Office365.Subscription.Office365SubscriptionModel>();
                    Dictionary<string, int> newSubcriptions = allSubcriptions.Where(x => !subscriptions.Any(s => s.Office365Offer.CloudPlusProductIdentifier.Contains(x.Key))).ToDictionary(x => x.Key, y => y.Value);
                    if (newSubcriptions?.Count > 0)
                    {
                        builder.AddActivity("CreateSecurityGroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateSecurityGroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.SecurityGroupName
                        });

                        builder.AddActivity("CreateDatabaseSecurityGroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseSecurityGroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.SecurityGroupName
                        });

                        builder.AddActivity("CreateDistributionGroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDistributionGroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.DistributionGroupName
                        });
                        builder.AddActivity("CreateDatabaseDistributionGroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseDistributionGroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.DistributionGroupName
                        });
                        builder.AddActivity("CreateOffice365GroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateO365GroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.Office365GroupName
                        });

                        builder.AddActivity("CreateDatabaseOffice365GroupMember", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseO365GroupMemberActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            context.Message.Users,
                            context.Message.Office365GroupName
                        });

                        builder.AddActivity("CreateMultiDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IMultiDatabaseCustomerSubscriptionActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            CloudPlusProductIdentifiers = newSubcriptions
                        });

                        builder.AddActivity("CreateOrderWithMultiItems", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderWithMultiItemsActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            CloudPlusProductIdentifiers = newSubcriptions
                        });

                        //builder.AddActivity("UpdateMultiDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionActivity)), new
                        //{
                        //    office365Customer.Office365CustomerId,
                        //    CloudPlusProductIdentifiers = newSubcriptions
                        //});

                        //builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        //{
                        //    SubscriptionState = Office365SubscriptionState.AvailableForOperations

                        //});



                    }
                    if (newSubcriptions.Count != message.CloudPlusProductIdentifiers.Count())
                    {
                        var oldSubcriptions = allSubcriptions.Where(x => subscriptions.Any(s => s.Office365Offer.CloudPlusProductIdentifier.Contains(x.Key))).ToList();
                        //Update Existing Subscription
                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.OperationInProgress,
                            Office365SubscriptionIds = subscriptions.Where(x => oldSubcriptions.Any(s => s.Key == x.Office365Offer.CloudPlusProductIdentifier)).Select(x => x.Office365SubscriptionId)
                        });

                        if (subscriptions.Any(x => x.Suspended))
                        {
                            //NEED TO CHECK THIS LINE
                            subscriptions.Where(x => x.Suspended).ToList().ForEach(x => x.Quantity = x.Quantity != 1 ? x.Quantity - 1 : 1);
                            builder.AddActivity("ActivateMultiSuspendedDatabaseSubscriptionActivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateMultiSuspendedDatabaseSubscriptionActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => x.Office365SubscriptionId)
                            });

                            builder.AddActivity("ActivateMultiSuspendedPartnerPlatformSubscriptionAcivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateMultiSuspendedPartnerPlatformSubscriptionActivity)), new
                            {
                                office365Customer.Office365CustomerId,
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => x.Office365SubscriptionId)
                            });

                            builder.AddActivity("UpdateMultiDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)
                            });

                            builder.AddActivity("UpdateMultiPartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiPartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value),
                                office365Customer.Office365CustomerId

                            });
                        }
                        else
                        {
                            builder.AddActivity("UpdateMultiDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)
                            });

                            builder.AddActivity("UpdateMultiPartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiPartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value),
                                office365Customer.Office365CustomerId

                            });

                        }

                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                            Office365SubscriptionIds = subscriptions.Select(x => x.Office365SubscriptionId)
                        });
                    }
                    foreach (var userSubcription in userSubcriptions)
                    {
                        var user = _userService.GetUser(userSubcription.Key);
                        foreach (var cpIdentifiers in userSubcription.Value.Split(','))
                        {
                            await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                           RoutingSlipEvents.Completed,
                           x => x.Send<IOffice365UserAssignLicenseCommand>(new
                           {
                               message.CompanyId,
                                   //TAG
                               CloudPlusProductIdentifier = cpIdentifiers,
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
                               message.UserRoles
                           }));
                        }

                    }

                    builder.AddSubscription(Office365ServiceConstants.RoutingSlipEventObserverUri,
                    RoutingSlipEvents.ActivityCompleted |
                    RoutingSlipEvents.ActivityFaulted |
                    RoutingSlipEvents.ActivityCompensated |
                    RoutingSlipEvents.ActivityCompensationFailed);


                    var routingSlip = builder.Build();

                    await context.Execute(routingSlip);
                }


            }
            else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.EditUser)
            {
                var o365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == message.Users.FirstOrDefault().UserPrincipalName);
                if (o365User == null)
                {
                    if (o365User == null || !o365User.Licenses.Any())
                        subscriptions.ForEach(x => allSubcriptions[x.Office365Offer.CloudPlusProductIdentifier] += 1);
                }
                if (o365User.Licenses.Any())
                {
                    o365User.Licenses.Where(l => !message.CloudPlusProductIdentifiers.Contains(l.Office365Offer.CloudPlusProductIdentifier)).ToList()
                        .ForEach(x => allSubcriptions[x.Office365Offer.CloudPlusProductIdentifier] += 1);
                }
                if (allSubcriptions.Count > 0 && allSubcriptions.Sum(x => x.Value) > 0)
                {
                    if (subscriptions == null)
                        subscriptions = new List<Models.Office365.Subscription.Office365SubscriptionModel>();
                    var newSubcriptions = allSubcriptions.Where(x => !subscriptions.Any(s => s.Office365Offer.CloudPlusProductIdentifier.Contains(x.Key))).ToList();
                    //ADD
                    if (newSubcriptions?.Count > 0)
                    {
                        builder.AddActivity("CreateMultiDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IMultiDatabaseCustomerSubscriptionActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            CloudPlusProductIdentifiers = newSubcriptions
                        });

                        builder.AddActivity("CreateOrderWithMultiItems", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderWithMultiItemsActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            CloudPlusProductIdentifiers = newSubcriptions
                        });

                        builder.AddActivity("UpdateMultiDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            CloudPlusProductIdentifiers = newSubcriptions
                        });

                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                            Office365SubscriptionIds = new List<string>()
                        });

                    }
                    //EDIT
                    if (newSubcriptions.Count != message.CloudPlusProductIdentifiers.Count())
                    {
                        var oldSubcriptions = allSubcriptions.Where(x => subscriptions.Any(s => s.Office365Offer.CloudPlusProductIdentifier.Contains(x.Key))).ToList();
                        //Update Existing Subscription
                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.OperationInProgress,
                            Office365SubscriptionIds = subscriptions.Where(x => oldSubcriptions.Any(s => s.Key == x.Office365Offer.CloudPlusProductIdentifier)).Select(x => x.Office365SubscriptionId)
                        });

                        if (subscriptions.Any(x => x.Suspended))
                        {
                            //NEED TO CHECK THIS LINE
                            subscriptions.Where(x => x.Suspended).ToList().ForEach(x => x.Quantity = x.Quantity != 1 ? x.Quantity - 1 : 1);
                            builder.AddActivity("ActivateMultiSuspendedDatabaseSubscriptionActivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateMultiSuspendedDatabaseSubscriptionActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => x.Office365SubscriptionId)
                            });

                            builder.AddActivity("ActivateMultiSuspendedPartnerPlatformSubscriptionAcivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateMultiSuspendedPartnerPlatformSubscriptionActivity)), new
                            {
                                office365Customer.Office365CustomerId,
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => x.Office365SubscriptionId)
                            });

                            builder.AddActivity("UpdateMultiDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)
                            });

                            builder.AddActivity("UpdateMultiPartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiPartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value),
                                office365Customer.Office365CustomerId

                            });
                        }
                        else
                        {
                            builder.AddActivity("UpdateMultiDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)
                            });

                            builder.AddActivity("UpdateMultiPartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiPartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptions.Where(x => x.Suspended).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value),
                                office365Customer.Office365CustomerId

                            });

                        }

                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                            Office365SubscriptionIds = subscriptions.Select(x => x.Office365SubscriptionId)
                        });
                    }
                    //DELETE
                    var subscriptionToRemove = subscriptions.Where(x => !allSubcriptions.Any(a => a.Key == x.Office365Offer.CloudPlusProductIdentifier));
                    if (subscriptionToRemove.Count() > 0)
                    {
                        builder.AddActivity("UpdateMultiDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.OperationInProgress,
                            Office365SubscriptionIds = subscriptionToRemove.Select(x => x.Office365SubscriptionId).ToList()
                        });
                        if (subscriptionToRemove.Any(x => x.Quantity == 1))
                        {
                            builder.AddActivity("SuspendMultiPartnerPlatformSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendMultiPartnerPlatformSubscriptionActivity)), new
                            {
                                office365Customer.Office365CustomerId,
                                Office365SubscriptionIds = subscriptionToRemove.Where(x => x.Quantity == 1).Select(x => x.Office365SubscriptionId).ToList()
                            });

                            builder.AddActivity("SuspendMultiDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendMultiDatabasesubscriptionActivity)), new
                            {
                                Office365SubscriptionIds = subscriptionToRemove.Where(x => x.Quantity == 1).Select(x => x.Office365SubscriptionId).ToList()
                            });
                        }
                        else
                        {
                            builder.AddActivity("UpdateMultiDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiDatabaseSubscriptionQuantityActivity)), new
                            {
                                Office365SubscriptionIds = subscriptionToRemove.Where(x => x.Quantity != 1).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)
                            });

                            builder.AddActivity("UpdateMultiPartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateMultiPartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                office365Customer.Office365CustomerId,
                                Office365SubscriptionIds = subscriptionToRemove.Where(x => x.Quantity != 1).Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value)

                            });
                        }
                    }
                }
            }
            else
            {
                foreach (var messageUserPrincipalName in message.Users)
                {
                    var o365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == messageUserPrincipalName.UserPrincipalName);
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
                    //TAG
                    if (o365User.Licenses.FirstOrDefault(l => l.Office365Offer.CloudPlusProductIdentifier == message.CloudPlusProductIdentifiers.FirstOrDefault()) == null)
                    {
                        newSubscriptionChangeCount++;
                    }
                }
                if (newSubscriptionChangeCount > 0)
                {
                    if (subscriptions == null)
                    {
                        builder.AddActivity("CreateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseSubscriptionActivity)), new
                        {
                            office365Customer.Office365CustomerId,
                            //TAG
                            CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),
                            Quantity = newSubscriptionChangeCount
                        });

                        builder.AddActivity("CreateOrder", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderActivity)), new
                        {
                            //TAG
                            CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),
                            office365Customer.Office365CustomerId,
                            Quantity = newSubscriptionChangeCount
                        });

                        builder.AddActivity("UpdateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionActivity)), new
                        {
                            message.CloudPlusProductIdentifiers,
                            office365Customer.Office365CustomerId
                        });

                        builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.AvailableForOperations
                        });
                    }
                    else
                    {
                        builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.OperationInProgress,
                            //TAG
                            subscriptions.FirstOrDefault().Office365SubscriptionId
                        });

                        if (subscriptions.FirstOrDefault().Suspended)
                        {
                            builder.AddActivity("ActivateSuspendedDatabaseSubscriptionActivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateSuspendedDatabaseSubscriptionActivity)), new
                            {
                                //TAG
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });

                            builder.AddActivity("ActivateSuspendedPartnerPlatformSubscriptionAcivity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IActivateSuspendedPartnerPlatformSubscriptionAcivity)), new
                            {
                                //TAG
                                subscriptions.FirstOrDefault().Office365CustomerId,
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });

                            builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = newSubscriptionChangeCount - 1, // well there is always 1 subscription left
                                                                                 //TAG
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });

                            builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = newSubscriptionChangeCount - 1, // well there is always 1 subscription left
                                //TAG
                                subscriptions.FirstOrDefault().Office365CustomerId,
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });
                        }
                        else
                        {

                            builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = newSubscriptionChangeCount,
                                //TAG
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });

                            builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = newSubscriptionChangeCount,
                                //TAG
                                subscriptions.FirstOrDefault().Office365CustomerId,
                                subscriptions.FirstOrDefault().Office365SubscriptionId
                            });

                        }

                        builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                            //TAG
                            subscriptions.FirstOrDefault().Office365SubscriptionId
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
                                //TAG
                                CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),
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
                                message.UserRoles
                            }));
                    }
                    else if (message.MessageType == ManageSubsctiptionAndLicenceCommandType.ChangeLicence)
                    {
                        var office365User =
                            await _office365DbUserService.GetOffice365DatabaseUserAsync(message.Users
                                .FirstOrDefault().UserPrincipalName);

                        var decreaseSubscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                            office365Customer.Office365CustomerId,
                            office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier);

                        builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                        {
                            SubscriptionState = Office365SubscriptionState.OperationInProgress,
                            decreaseSubscription.Office365SubscriptionId
                        });

                        if (decreaseSubscription.Quantity == 1)
                        {
                            builder.AddActivity("SuspendPartnerPlatformSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                            {
                                decreaseSubscription.Office365CustomerId,
                                decreaseSubscription.Office365SubscriptionId
                            });

                            builder.AddActivity("SuspendDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendDatabasesubscriptionActivity)), new
                            {
                                decreaseSubscription.Office365SubscriptionId
                            });
                        }
                        else
                        {
                            builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = -1,
                                decreaseSubscription.Office365SubscriptionId
                            });

                            builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                            {
                                QuantityChange = -1,
                                decreaseSubscription.Office365CustomerId,
                                decreaseSubscription.Office365SubscriptionId
                            });

                            builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                                decreaseSubscription.Office365SubscriptionId
                            });
                        }


                        await builder.AddSubscription(Office365ServiceConstants.Office365UserChangeLicenseUri,
                            RoutingSlipEvents.Completed,
                            x => x.Send<IOffice365UserChangeLicenseCommand>(new
                            {
                                message.CompanyId,
                                office365Customer.Office365CustomerId,
                                //TAG
                                CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),
                                office365User.UserPrincipalName,
                                //TAG
                                AssignCloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),
                                RemoveCloudPlusProductIdentifier = office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier,
                                message.UserRoles
                            }));
                    }
                    else
                    {
                        var supscriptionsToChange = new Dictionary<string, int>();
                        foreach (var messageUserPrincipalName in message.Users)
                        {
                            var office365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == messageUserPrincipalName.UserPrincipalName);

                            if (office365User != null)
                            {
                                if (office365User.Licenses.Any())
                                {
                                    if (office365User.Licenses.FirstOrDefault(l =>
                                            l.Office365Offer.CloudPlusProductIdentifier ==
                                            message.CloudPlusProductIdentifiers.FirstOrDefault()) == null)//TAG
                                    {
                                        await builder.AddSubscription(Office365ServiceConstants.Office365UserChangeLicenseUri,
                                            RoutingSlipEvents.Completed,
                                            x => x.Send<IOffice365UserChangeLicenseCommand>(new
                                            {
                                                message.CompanyId,
                                                office365Customer.Office365CustomerId,
                                                CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),//TAG
                                                office365User.UserPrincipalName,
                                                AssignCloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),//TAG
                                                RemoveCloudPlusProductIdentifier = office365User.Licenses.FirstOrDefault().Office365Offer.CloudPlusProductIdentifier,
                                                message.UserRoles
                                            }));
                                        var decreaseSubscription = await
                                            _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                                                office365Customer.Office365CustomerId,
                                                office365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier);

                                        if (supscriptionsToChange.ContainsKey(decreaseSubscription.Office365SubscriptionId))
                                            supscriptionsToChange[decreaseSubscription.Office365SubscriptionId]++;
                                        else
                                            supscriptionsToChange.Add(decreaseSubscription.Office365SubscriptionId, 1);

                                    }
                                }
                                else
                                {
                                    var user = _userService.GetUser(messageUserPrincipalName.UserPrincipalName);
                                    await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                                        RoutingSlipEvents.Completed,
                                        x => x.Send<IOffice365UserAssignLicenseCommand>(new
                                        {
                                            message.CompanyId,
                                            CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),//TAG
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
                                            message.UserRoles
                                        }));
                                }
                            }
                            else
                            {
                                var user = _userService.GetUser(messageUserPrincipalName.UserPrincipalName);
                                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                                    RoutingSlipEvents.Completed,
                                    x => x.Send<IOffice365UserAssignLicenseCommand>(new
                                    {
                                        message.CompanyId,
                                        CloudPlusProductIdentifier = message.CloudPlusProductIdentifiers.FirstOrDefault(),//TAG
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
                                        message.UserRoles
                                    }));
                            }
                        }
                        foreach (var subscriptionToChange in supscriptionsToChange)
                        {
                            var decreaseSubscription = await
                                _office365DbSubscriptionService.GetSubscriptionAsyc(subscriptionToChange.Key);

                            builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.OperationInProgress,
                                decreaseSubscription.Office365SubscriptionId
                            });

                            if (decreaseSubscription.Quantity == subscriptionToChange.Value)
                            {
                                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange = (decreaseSubscription.Quantity - 1) * -1, // well there is always 1 subscription left
                                    decreaseSubscription.Office365CustomerId,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                                builder.AddActivity("SuspendPartnerPlatformSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendPartnerPlatformSubscriptionActivity)), new
                                {
                                    decreaseSubscription.Office365CustomerId,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                                builder.AddActivity("SuspendDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ISuspendDatabasesubscriptionActivity)), new
                                {
                                    decreaseSubscription.Office365SubscriptionId
                                });
                            }
                            else
                            {
                                builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange = subscriptionToChange.Value * -1,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                                {
                                    QuantityChange = subscriptionToChange.Value * -1,
                                    decreaseSubscription.Office365CustomerId,
                                    decreaseSubscription.Office365SubscriptionId
                                });

                            }

                            builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                            {
                                SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                                decreaseSubscription.Office365SubscriptionId
                            });
                        }
                    }

                    builder.AddSubscription(Office365ServiceConstants.RoutingSlipEventObserverUri,
                        RoutingSlipEvents.ActivityCompleted |
                        RoutingSlipEvents.ActivityFaulted |
                        RoutingSlipEvents.ActivityCompensated |
                        RoutingSlipEvents.ActivityCompensationFailed);


                    var routingSlip = builder.Build();

                    await context.Execute(routingSlip);
                }
            }
        }
    }
}