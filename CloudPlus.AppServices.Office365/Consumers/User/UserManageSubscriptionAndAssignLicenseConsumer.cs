using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
using CloudPlus.QueueModels.Office365.User.Commands;
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
    public interface IUserManageSubscriptionAndAssignLicense : IConsumer<IUserManageSubscriptionAndAssignLicenseCommand>
    {

    }

    public interface IUserManageSubscriptionAndAssignLicenseCommand
    {
        int CompanyId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        string UserPrincipalName { get; set; }
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UsageLocation { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        string State { get; set; }
        string StreetAddress { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }

    public class UserManageSubscriptionAndAssignLicenseConsumer : IUserManageSubscriptionAndAssignLicense
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IActivityConfigurator _activityConfigurator;

        public UserManageSubscriptionAndAssignLicenseConsumer(
            IOffice365DbCustomerService office365DbCustomerService,
            IOffice365DbSubscriptionService office365DbSubscriptionService, 
            IActivityConfigurator activityConfigurator)
        {
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _activityConfigurator = activityConfigurator;
        }
        public async Task Consume(ConsumeContext<IUserManageSubscriptionAndAssignLicenseCommand> context)
        {
            var message = context.Message;
            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(message.CompanyId);

            var subscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
                office365Customer.Office365CustomerId, message.CloudPlusProductIdentifier);

            if (subscription == null)
            {
                builder.AddActivity("CreateDatabaseSubscription", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateDatabaseSubscriptionActivity)), new
                {
                    office365Customer.Office365CustomerId,
                    message.CloudPlusProductIdentifier,
                    Quantity = 1
                });

                builder.AddActivity("CreateOrder", _activityConfigurator.GetActivityExecuteUri(context, typeof(ICreateOrderActivity)), new
                {
                    message.CloudPlusProductIdentifier,
                    office365Customer.Office365CustomerId,
                    Quantity = 1
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
                    QuantityChange = 1,
                    subscription.Office365SubscriptionId
                });

                builder.AddActivity("UpdatePartnerPlatformSubscriptionQuantity", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdatePartnerPlatformSubscriptionQuantityActivity)), new
                {
                    QuantityChange = 1,
                    subscription.Office365CustomerId,
                    subscription.Office365SubscriptionId
                });

                builder.AddActivity("UpdateDatabaseSubscriptionState", _activityConfigurator.GetActivityExecuteUri(context, typeof(IUpdateDatabaseSubscriptionStateActivity)), new
                {
                    SubscriptionState = Office365SubscriptionState.AvailableForOperations,
                    subscription.Office365SubscriptionId
                });
            }

            builder.AddSubscription(Office365ServiceConstants.QueueOffice365RoutingSlipEventUri,
                RoutingSlipEvents.ActivityCompleted |
                RoutingSlipEvents.ActivityFaulted |
                RoutingSlipEvents.ActivityCompensated |
                RoutingSlipEvents.ActivityCompensationFailed);
            
                await builder.AddSubscription(Office365ServiceConstants.QueueOffice365UserAssignLicenseUri,
                    RoutingSlipEvents.Completed,
                    x => x.Send<IOffice365UserAssignLicenseCommand>(new
                    {
                        message.CompanyId,
                        message.CloudPlusProductIdentifier,
                        message.UserPrincipalName,
                        message.DisplayName,
                        message.FirstName,
                        message.LastName,
                        UsageLocation = "US",
                        message.City,
                        message.Country,
                        message.PhoneNumber,
                        message.PostalCode,
                        message.State,
                        message.StreetAddress,
                        message.UserRoles
                    }));

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}