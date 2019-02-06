using CloudPlus.Logging;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiPartnerPlatformSubscriptionQuantity
{
    public class UpdateMultiPartnerPlatformSubscriptionQuantityActivity : IUpdateMultiPartnerPlatformSubscriptionQuantityActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;

        public UpdateMultiPartnerPlatformSubscriptionQuantityActivity(IOffice365SubscriptionService office365SubscriptionService)
        {
            _office365SubscriptionService = office365SubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateMultiPartnerPlatformSubscriptionQuantityArguments> context)
        {

            var arguments = context.Arguments;

            foreach (var item in arguments.Office365SubscriptionIds)
            {
                var subscription = await _office365SubscriptionService.UpdatePartnerPlatformSubscriptionQuantity(arguments.Office365CustomerId,
                item.Key, item.Value);
            }


            return context.Completed(new UpdateMultiPartnerPlatformSubscriptionQuantityLog
            {
                Office365SubscriptionIds = arguments.Office365SubscriptionIds,
                Office365CustomerId = arguments.Office365CustomerId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdateMultiPartnerPlatformSubscriptionQuantityLog> context)
        {
            try
            {
                var log = context.Log;
                foreach (var item in log.Office365SubscriptionIds)
                {
                    await _office365SubscriptionService.UpdatePartnerPlatformSubscriptionQuantity(log.Office365CustomerId,
                    item.Key, item.Value);
                    }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdatePartnerPlatformSubscriptionQuantityActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
