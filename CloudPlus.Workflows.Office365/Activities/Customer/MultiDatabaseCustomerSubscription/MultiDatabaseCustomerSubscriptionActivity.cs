using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription
{
    public class MultiDatabaseCustomerSubscriptionActivity : IMultiDatabaseCustomerSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public MultiDatabaseCustomerSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IMultiDatabaseCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var Subscriptions = new List<Models.Office365.Subscription.Office365SubscriptionModel>();
            
            foreach (var CloudPlusProductIdentifier in arguments.CloudPlusProductIdentifiers)
            {
                var model = new Models.Office365.Subscription.Office365SubscriptionModel
                {
                    Office365CustomerId = arguments.Office365CustomerId,
                    Office365FriendlyName = string.Empty,
                    Office365Offer = new Models.Office365.Offer.Office365OfferModel { CloudPlusProductIdentifier = CloudPlusProductIdentifier.Key },
                    Quantity = CloudPlusProductIdentifier.Value,
                    SubscriptionState = Enums.Office365.Office365SubscriptionState.OperationInProgress
                };
                await _office365DbSubscriptionService.CreateSubscription(model);
                Subscriptions.Add(model);
            }

            return context.Completed(new MultiDatabaseCustomerSubscriptionLog
            {
                Subscriptions = Subscriptions
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IMultiDatabaseCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                foreach (var subscription in logs.Subscriptions)
                {
                    await _office365DbSubscriptionService.DeleteSubscription(subscription.Office365SubscriptionId);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Compensate MultiDatabaseCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
