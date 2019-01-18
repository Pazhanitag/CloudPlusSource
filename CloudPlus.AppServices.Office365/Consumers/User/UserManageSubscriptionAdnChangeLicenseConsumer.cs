using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Subscription;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription;
using CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__;
using CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public interface IUserManageSubscriptionAdnChangeLicenseConsumer : IConsumer<IUserManageSubscriptionAdnChangeLicenseConsumerCommand> { }

    public interface IUserManageSubscriptionAdnChangeLicenseConsumerCommand
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
        string RemoveCloudPlusProductIdentifier { get; set; }
        string AssignCloudPlusProductIdentifier { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }

    public class UserManageSubscriptionAdnChangeLicenseConsumer : IUserManageSubscriptionAdnChangeLicenseConsumer
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IActivityConfigurator _activityConfigurator;

        public UserManageSubscriptionAdnChangeLicenseConsumer(IOffice365DbCustomerService office365DbCustomerService, IOffice365DbSubscriptionService office365DbSubscriptionService, IActivityConfigurator activityConfigurator)
        {
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _activityConfigurator = activityConfigurator;
        }

        public async Task Consume(ConsumeContext<IUserManageSubscriptionAdnChangeLicenseConsumerCommand> context)
        {
            var message = context.Message;
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(message.CompanyId);

            var subscriptionToRemove = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                office365Customer.Office365CustomerId, message.RemoveCloudPlusProductIdentifier);

            var subscriptionToAdd = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                office365Customer.Office365CustomerId, message.AssignCloudPlusProductIdentifier);

            builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
            {
                SubscriptionState = Office365SubscriptionState.OperationInProgress,
                subscriptionToRemove.Office365SubscriptionId
            });
            // TODO: Add activity to spend subscription if quantity 1 instead of changing quantity ??


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

            builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
            {
                SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                subscriptionToRemove.Office365SubscriptionId
            });

            if (subscriptionToAdd == null)
            {
                builder.AddActivity("CreateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseSubscriptionActivity)), new
                {
                    office365Customer.Office365CustomerId,
                    CloudPlusProductIdentifier = message.AssignCloudPlusProductIdentifier,
                    Quantity = 1
                });

                builder.AddActivity("CreateOrder", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderActivity)), new
                {
                    CloudPlusProductIdentifier = message.AssignCloudPlusProductIdentifier,
                    office365Customer.Office365CustomerId,
                    Quantity = 1
                });

                builder.AddActivity("UpdateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionActivity)), new
                {
                    CloudPlusProductIdentifier = message.AssignCloudPlusProductIdentifier,
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
                    subscriptionToAdd.Office365SubscriptionId
                });

                builder.AddActivity("UpdateDatabaseSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionQuantityActivity)), new
                {
                    QuantityChange = 1,
                    subscriptionToAdd.Office365SubscriptionId
                });

                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                {
                    QuantityChange = 1,
                    subscriptionToAdd.Office365CustomerId,
                    subscriptionToAdd.Office365SubscriptionId
                });

                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                    subscriptionToAdd.Office365SubscriptionId
                });
            }

            // TODO: Add activities for user licencse add/remove

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}