using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendDatabasesubscription
{
    public class SuspendDatabasesubscriptionActivity : ISuspendDatabasesubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public SuspendDatabasesubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ISuspendDatabasesubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var existingSubscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(arguments.Office365SubscriptionId);

            await _office365DbSubscriptionService.SuspendSubscription(arguments.Office365SubscriptionId);


            return context.Completed(new SuspendDatabasesubscriptionLog
            {
                Office365SubscriptionId = existingSubscription.Office365SubscriptionId,
                Quantity = existingSubscription.Quantity
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ISuspendDatabasesubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.UnsuspendSubscription(log.Office365SubscriptionId);
                await _office365DbSubscriptionService.UpdateSubscriptionQuantity(log.Office365SubscriptionId, log.Quantity);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DeleteDatabaseSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}