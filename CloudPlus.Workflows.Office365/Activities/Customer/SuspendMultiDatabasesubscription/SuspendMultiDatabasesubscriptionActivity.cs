using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiDatabasesubscription
{
    public class SuspendMultiDatabasesubscriptionActivity : ISuspendMultiDatabasesubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public SuspendMultiDatabasesubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ISuspendMultiDatabasesubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var existingSubscription = await _office365DbSubscriptionService.GetSubscriptionListAsyc(arguments.Office365SubscriptionIds);

            await _office365DbSubscriptionService.SuspendSubscriptionList(arguments.Office365SubscriptionIds);


            return context.Completed(new SuspendMultiDatabasesubscriptionLog
            {
                Office365SubscriptionIds = existingSubscription.Select(x => new KeyValuePair<string, int>(x.Office365SubscriptionId, x.Quantity)).ToDictionary(x => x.Key, x => x.Value),
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ISuspendMultiDatabasesubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.UnsuspendSubscriptionList(log.Office365SubscriptionIds.Select(x => x.Key).ToList());
                foreach (var item in log.Office365SubscriptionIds)
                {
                    await _office365DbSubscriptionService.UpdateSubscriptionQuantity(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DeleteDatabaseSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
