using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionQuantity
{
    public class UpdateMultiDatabaseSubscriptionQuantityActivity : IUpdateMultiDatabaseSubscriptionQuantityActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateMultiDatabaseSubscriptionQuantityActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateMultiDatabaseSubscriptionQuantityArguments> context)
        {

            var arguments = context.Arguments;

            foreach (var item in arguments.Office365SubscriptionIds)
            {
                var subscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(item.Key);

                if (subscription == null)
                    throw new NullReferenceException($"There is no subscription with office 365 subscription id {item.Key} in database");

                await _office365DbSubscriptionService.UpdateSubscriptionQuantity(item.Key,
                    item.Value);
            }


            return context.Completed(new UpdateMultiDatabaseSubscriptionQuantityLog
            {
                Office365SubscriptionIds = arguments.Office365SubscriptionIds
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdateMultiDatabaseSubscriptionQuantityLog> context)
        {
            try
            {
                var log = context.Log;
                foreach (var item in log.Office365SubscriptionIds)
                {
                    var subscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(item.Key);

                    if (subscription == null)
                        throw new NullReferenceException($"There is no subscription with office 365 subscription id {item.Key} in database");

                    await _office365DbSubscriptionService.UpdateSubscriptionQuantity(item.Key,
                        item.Value);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdateMultiDatabaseSubscriptionQuantityActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
