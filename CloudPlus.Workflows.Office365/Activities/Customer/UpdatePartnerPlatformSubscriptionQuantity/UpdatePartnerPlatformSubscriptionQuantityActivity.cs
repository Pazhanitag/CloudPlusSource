using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdatePartnerPlatformSubscriptionQuantity
{
    public class UpdatePartnerPlatformSubscriptionQuantityActivity : IUpdatePartnerPlatformSubscriptionQuantityActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public UpdatePartnerPlatformSubscriptionQuantityActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdatePartnerPlatformSubscriptionQuantityArguments> context)
        {

            var arguments = context.Arguments;
            
            var subscription = await _office365SubscriptionService.UpdatePartnerPlatformSubscriptionQuantity(arguments.Office365CustomerId,
                arguments.Office365SubscriptionId, arguments.QuantityChange);

            return context.Completed(new UpdatePartnerPlatformSubscriptionQuantityLog 
            {
                Office365SubscriptionId = arguments.Office365SubscriptionId,
                Office365CustomerId = arguments.Office365CustomerId,
                QuantityChange = subscription.Quantity - arguments.QuantityChange
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdatePartnerPlatformSubscriptionQuantityLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365SubscriptionService.UpdatePartnerPlatformSubscriptionQuantity(log.Office365CustomerId,
                    log.Office365SubscriptionId, log.QuantityChange);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdatePartnerPlatformSubscriptionQuantityActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}