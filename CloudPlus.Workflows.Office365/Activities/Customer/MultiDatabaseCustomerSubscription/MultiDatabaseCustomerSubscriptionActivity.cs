using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;

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

            foreach (var subscription in arguments.Subscriptions)
            {
                await _office365DbSubscriptionService.CreateSubscription(subscription);
            }

            return context.Completed(new MultiDatabaseCustomerSubscriptionLog
            {
                Subscriptions = arguments.Subscriptions
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
