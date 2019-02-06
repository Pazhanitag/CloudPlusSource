using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription
{
    public class ActivateMultiSuspendedDatabaseSubscriptionActivity : IActivateMultiSuspendedDatabaseSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public ActivateMultiSuspendedDatabaseSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IActivateMultiSuspendedDatabaseSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var existingSubscription = await _office365DbSubscriptionService.GetSubscriptionListAsyc(arguments.Office365SubscriptionIds);

            await _office365DbSubscriptionService.UnsuspendSubscriptionList(existingSubscription.Select(x=>x.Office365SubscriptionId).ToList());


            return context.Completed(new ActivateMultiSuspendedDatabaseSubscriptionLog
            {
                Office365SubscriptionIds = existingSubscription.Select(x => x.Office365SubscriptionId).ToList()
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IActivateMultiSuspendedDatabaseSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.SuspendSubscriptionList(log.Office365SubscriptionIds);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating ActivateMultiSuspendedSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
