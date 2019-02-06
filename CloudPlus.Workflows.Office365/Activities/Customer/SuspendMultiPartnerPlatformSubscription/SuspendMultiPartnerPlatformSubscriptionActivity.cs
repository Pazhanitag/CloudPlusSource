using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiPartnerPlatformSubscription
{
    public class SuspendMultiPartnerPlatformSubscriptionActivity : ISuspendMultiPartnerPlatformSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public SuspendMultiPartnerPlatformSubscriptionActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ISuspendMultiPartnerPlatformSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            await _office365SubscriptionService.SuspendMultiPartnerPlatformSubscriptionAsync(arguments.Office365CustomerId,
                arguments.Office365SubscriptionIds);

            return context.Completed(new SuspendMultiPartnerPlatformSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365SubscriptionIds = arguments.Office365SubscriptionIds
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ISuspendMultiPartnerPlatformSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365SubscriptionService.ActivateMultiSuspendedPartnerPlatformSubscriptionAsync(
                    log.Office365CustomerId, log.Office365SubscriptionIds);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating SuspendMultiPartnerPlatformSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
