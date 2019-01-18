using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription
{
    public class SuspendPartnerPlatformSubscriptionActivity : ISuspendPartnerPlatformSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public SuspendPartnerPlatformSubscriptionActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ISuspendPartnerPlatformSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(arguments.Office365CustomerId,
                arguments.Office365SubscriptionId);

            return context.Completed(new SuspendPartnerPlatformSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365SubscriptionId = arguments.Office365SubscriptionId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ISuspendPartnerPlatformSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;

                await _office365SubscriptionService.ActivateSuspendedPartnerPlatformSubscriptionAsync(
                    log.Office365CustomerId, log.Office365SubscriptionId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating SuspendPartnerPlatformSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}