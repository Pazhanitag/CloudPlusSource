using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedPartnerPlatformSubscription
{
    public class ActivateMultiSuspendedPartnerPlatformSubscriptionActivity : IActivateMultiSuspendedPartnerPlatformSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public ActivateMultiSuspendedPartnerPlatformSubscriptionActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IActivateMultiSuspendedPartnerPlatformSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            await _office365SubscriptionService.ActivateMultiSuspendedPartnerPlatformSubscriptionAsync(
                arguments.Office365CustomerId, arguments.Office365SubscriptionIds);

            return context.Completed(new ActivateMultiSuspendedPartnerPlatformSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365SubscriptionIds = arguments.Office365SubscriptionIds
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IActivateMultiSuspendedPartnerPlatformSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365SubscriptionService.SuspendMultiPartnerPlatformSubscriptionAsync(log.Office365CustomerId,
                    log.Office365SubscriptionIds);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for ActivateMultiSuspendedPartnerPlatformSubscriptionActivity.", ex);
            }

            return context.Compensated();
        }
    }
}
