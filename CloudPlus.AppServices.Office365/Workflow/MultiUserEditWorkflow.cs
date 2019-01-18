using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Identity;
using CloudPlus.Models.Office365.User;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Subscription;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Identity.User;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;

namespace CloudPlus.AppServices.Office365.Workflow
{
    public interface IMultiUserEditWorkflow : IWorkflow<ConsumeContext<IOffice365UserMultiEditCommand>>
    {

    }
    public class MultiUserEditWorkflow : IMultiUserEditWorkflow
    {
        private readonly IUserService _userService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IActivityConfigurator _activityConfigurator;

        public MultiUserEditWorkflow(IUserService userService, IOffice365DbCustomerService office365DbCustomerService, IOffice365DbUserService office365DbUserService, IOffice365DbSubscriptionService office365DbSubscriptionService, IActivityConfigurator activityConfigurator)
        {
            _userService = userService;
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbUserService = office365DbUserService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Execute(ConsumeContext<IOffice365UserMultiEditCommand> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            var message = context.Message;

            var users = _userService.GetUsers(message.CompanyId).ToList();

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(message.CompanyId);

            var office365Users = await _office365DbUserService.GetAllCustomerUsersWithLicensesAndOfferAsync(office365Customer.Id);

            var subscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                office365Customer.Office365CustomerId, message.CloudPlusProductIdentifier);

            if (subscription == null)
            {
                builder.AddActivity("CreateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseSubscriptionActivity)), new
                {
                    office365Customer.Office365CustomerId,
                    message.CloudPlusProductIdentifier,
                    Quantity = message.UserPrincipalNames.Count()
                });
                
                builder.AddActivity("CreateOrder", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderActivity)), new
                {
                    message.CloudPlusProductIdentifier,
                    office365Customer.Office365CustomerId,
                    Quantity = message.UserPrincipalNames.Count()
                });

                builder.AddActivity("UpdateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionActivity)), new
                {
                    message.CloudPlusProductIdentifier,
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
                    subscription.Office365SubscriptionId
                });

                builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                {
                    QuantityChange = message.UserPrincipalNames.Count(),
                    subscription.Office365SubscriptionId
                });

                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                {
                    QuantityChange = message.UserPrincipalNames.Count(),
                    subscription.Office365CustomerId,
                    subscription.Office365SubscriptionId
                });

                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                    subscription.Office365SubscriptionId
                });
            }

            var subscriptionsToChange = new Dictionary<string, int>();
            var usersToAssignLicences = new List<UserModel>();
            var usersToChangeRoles = new List<UserModel>();

            foreach (var principalName in message.UserPrincipalNames)
            {
                var user = users.FirstOrDefault(u => u.UserName == principalName);

                // If there is no user, pass this iteration
                if (user == null) continue;

                var office365User = office365Users.FirstOrDefault(u => u.UserPrincipalName == principalName);

                if (office365User == null)
                {
                    usersToAssignLicences.Add(user);
                }
                else
                {
                    if (office365User.Office365UserState == Office365UserState.Inactive) continue;

                    var license = office365User.Licenses.FirstOrDefault(l =>
                        l.Office365Offer.CloudPlusProductIdentifier == message.CloudPlusProductIdentifier);

                    if (license != null)
                    {
                        var subscriptionToDecrease =
                            await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                                office365Customer.Office365CustomerId,
                                license.Office365Offer.CloudPlusProductIdentifier);

                        if (subscriptionsToChange.ContainsKey(subscriptionToDecrease.Office365SubscriptionId))
                        {
                            subscriptionsToChange[subscriptionToDecrease.Office365SubscriptionId]++;
                        }
                        else
                        {
                            subscriptionsToChange.Add(subscriptionToDecrease.Office365SubscriptionId, 1);
                        }
                   
                    }
                    else
                    {
                        usersToChangeRoles.Add(user);
                    }
                }
            }

            //foreach (var subscriptionToChange in subscriptionsToChange)
            //{
            //    builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionActivity)), new
            //    {
            //        SubscriptionState = Office365SubscriptionState.OperationInProgress,
            //        Office365SubscriptionId = subscriptionToChange.Key
            //    });

            //    builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
            //    {
            //        QuantityChange = subscriptionToChange.Value * -1,
            //        Office365SubscriptionId = subscriptionToChange.Key
            //    });

            //    builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
            //    {
            //        QuantityChange = subscriptionToChange.Value * -1,
            //        office365Customer.Office365CustomerId,
            //        Office365SubscriptionId = subscriptionToChange.Key
            //    });

            //    builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionActivity)), new
            //    {
            //        SubscriptionState = Office365SubscriptionState.AvailableForOperations,
            //        Office365SubscriptionId = subscriptionToChange.Key
            //    });
            //}

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);


            foreach (var usersToAssignLicence in usersToAssignLicences)
            {
                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                    RoutingSlipEvents.Completed,
                    x => x.Send<IOffice365UserAssignLicenseCommand>(new
                    {
                        message.CompanyId,
                        message.CloudPlusProductIdentifier,
                        UserPrincipalName = usersToAssignLicence.UserName,
                        usersToAssignLicence.DisplayName,
                        usersToAssignLicence.FirstName,
                        usersToAssignLicence.LastName,
                        UsageLocation = "US",
                        usersToAssignLicence.City,
                        usersToAssignLicence.Country,
                        usersToAssignLicence.PhoneNumber,
                        PostalCode = usersToAssignLicence.ZipCode,
                        usersToAssignLicence.State,
                        usersToAssignLicence.StreetAddress,
                        message.UserRoles
                    }));
            }
           
            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }

        private async void SendUserAssignLicense(ConsumeContext<IOffice365UserMultiEditCommand> context, UserModel user)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri);

            await assignLicenseEndpoint.Send<IOffice365UserAssignLicenseCommand>(new
            {
                messages.CompanyId,
                messages.CloudPlusProductIdentifier,
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
                messages.UserRoles
            });
        }

        private async void SendUserChangeLicense(ConsumeContext<IOffice365UserMultiEditCommand> context, Office365UserModel office365User)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.Office365UserChangeLicenseUri);

            await assignLicenseEndpoint.Send<IOffice365UserChangeLicenseCommand>(new
            {
                messages.CompanyId,
                office365User.UserPrincipalName,
                RemoveCloudPlusProductIdentifier = office365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier,
                AssignCloudPlusProductIdentifier = messages.CloudPlusProductIdentifier,
                messages.UserRoles
            });
        }

        private async void SendUseChangeRoles(ConsumeContext<IOffice365UserMultiEditCommand> context, UserModel user)
        {
            var messages = context.Message;

            var assignLicenseEndpoint = await context.GetSendEndpoint(Office365ServiceConstants.QueueOffice365ChangeUserRolesUri);

            await assignLicenseEndpoint.Send<IOffice365UserChangeRolesCommand>(new
            {
                messages.CompanyId,
                UserPrincipalName = user.UserName,
                messages.UserRoles
            });
        }
    }
}