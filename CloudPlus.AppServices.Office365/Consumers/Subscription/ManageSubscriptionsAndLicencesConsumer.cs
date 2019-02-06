using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.ManageSubscription;
using CloudPlus.Constants;
using CloudPlus.Enums.Office365;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using CloudPlus.QueueModels.Office365.User.Commands;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Subscription;
using CloudPlus.Services.Database.Office365.User;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Subscription
{
    public class ManageSubscriptionsAndLicencesConsumer : IManageSubscriptionsAndLicencesConsumer
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;
        private readonly IOffice365DbUserService _office365DbUserService;

        private readonly IManageSubscriptionWorkflow _manageSubscriptionWorkflow;
        public ManageSubscriptionsAndLicencesConsumer(IOffice365DbCustomerService office365DbCustomerService, IOffice365DbSubscriptionService office365DbSubscriptionService, IManageSubscriptionWorkflow manageSubscriptionWorkflow, IOffice365DbUserService office365DbUserService)
        {
            _office365DbCustomerService = office365DbCustomerService;
            _office365DbSubscriptionService = office365DbSubscriptionService;
            _manageSubscriptionWorkflow = manageSubscriptionWorkflow;
            _office365DbUserService = office365DbUserService;
        }
        public async Task Consume(ConsumeContext<IManageSubscriptionsAndLicencesCommand> context)
        {
            var messages = context.Message;
            string productIdentifier;
            if (messages.MessageType == ManageSubsctiptionAndLicenceCommandType.MultiAddUser)
            {
               
                    await _manageSubscriptionWorkflow.Execute(context);
              
            }
            //if (string.IsNullOrWhiteSpace(messages.CloudPlusProductIdentifiers.FirstOrDefault()) 
            //    && messages.UserRoles != null && messages.UserRoles.Any() 
            //    && messages.Users != null && messages.Users.Any())
            //{
            //    foreach (var office365User in messages.Users)
            //    {
            //        var o365User =
            //            await _office365DbUserService.GetOffice365DatabaseUserAsync(office365User.UserPrincipalName);

            //        if (o365User != null)
            //        {
            //            await context.Send<IOffice365UserChangeRolesCommand>(Office365ServiceConstants.QueueOffice365ChangeUserRolesUri, new
            //            {
            //                messages.CompanyId,
            //                office365User.UserPrincipalName,
            //                messages.UserRoles
            //            });
            //        }
            //        else
            //        {
            //            await context.Send<IOffice365UserCreateCommand>(
            //                Office365ServiceConstants.QueueOffice365CreateUserUri, new
            //                {
            //                    messages.CompanyId,
            //                    office365User.UserPrincipalName,
            //                    office365User.Password,
            //                    UsageLocation = "US",
            //                    messages.UserRoles
            //                });
            //        }
            //        // Send assign roles
            //    }
            //    return;
            //}
            //            if (messages.MessageType == ManageSubsctiptionAndLicenceCommandType.RemoveLicence ||
            //                messages.MessageType == ManageSubsctiptionAndLicenceCommandType.RestoreUser ||
            //                messages.MessageType == ManageSubsctiptionAndLicenceCommandType.HardDeleteUser)
            //            {
            //                var o365User =
            //                    await _office365DbUserService.GetOffice365DatabaseUserAsync(messages.Users.FirstOrDefault()
            //                        .UserPrincipalName);

            //                productIdentifier = o365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier;
            //            }
            //            else
            //            {
            //                productIdentifier = messages.CloudPlusProductIdentifiers.FirstOrDefault();
            //            }

            //            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(messages.CompanyId);

            //            var subscription = await _office365DbSubscriptionService.GetSubscriptionByProductIdentifierAsync(
            //                office365Customer.Office365CustomerId, productIdentifier);

            //            if (subscription == null || subscription.SubscriptionState != Office365SubscriptionState.OperationInProgress)
            //            {
            //                 await _manageSubscriptionWorkflow.Execute(context);
            //            }
            //            else
            //            {

            //                if (context.GetRetryAttempt() < 100) // ????
            //                {
            //#pragma warning disable 4014
            //                    context.Redeliver(TimeSpan.FromSeconds(15));
            //#pragma warning restore 4014
            //                }
            //            }
        }
    }
}