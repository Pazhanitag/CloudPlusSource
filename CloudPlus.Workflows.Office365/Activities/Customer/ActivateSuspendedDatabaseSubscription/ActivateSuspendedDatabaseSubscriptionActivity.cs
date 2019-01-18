using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedDatabaseSubscription
{
    public class ActivateSuspendedDatabaseSubscriptionActivity : IActivateSuspendedDatabaseSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public ActivateSuspendedDatabaseSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IActivateSuspendedDatabaseSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var existingSubscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(arguments.Office365SubscriptionId);

            await _office365DbSubscriptionService.UnsuspendSubscription(arguments.Office365SubscriptionId);


            return context.Completed(new ActivateSuspendedDatabaseSubscriptionLog
            {
                Office365SubscriptionId = existingSubscription.Office365SubscriptionId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IActivateSuspendedDatabaseSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.SuspendSubscription(log.Office365SubscriptionId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating ActivateSuspendedSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}