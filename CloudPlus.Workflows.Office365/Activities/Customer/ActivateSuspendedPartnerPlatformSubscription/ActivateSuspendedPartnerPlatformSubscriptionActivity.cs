using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedPartnerPlatformSubscription
{
    public class ActivateSuspendedPartnerPlatformSubscriptionActivity : IActivateSuspendedPartnerPlatformSubscriptionAcivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public ActivateSuspendedPartnerPlatformSubscriptionActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IActivateSuspendedPartnerPlatformSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            await _office365SubscriptionService.ActivateSuspendedPartnerPlatformSubscriptionAsync(
                arguments.Office365CustomerId, arguments.Office365SubscriptionId);

            return context.Completed(new ActivateSuspendedPartnerPlatformSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365SubscriptionId = arguments.Office365SubscriptionId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IActivateSuspendedPartnerPlatformSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(log.Office365CustomerId,
                    log.Office365SubscriptionId);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for ActivateSuspendedPartnerPlatformSubscriptionActivity.", ex);
            }

            return context.Compensated();
        }
    }
}