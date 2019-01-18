using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity
{
    public class UpdateDatabaseSubscriptionQuantityActivity : IUpdateDatabaseSubscriptionQuantityActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateDatabaseSubscriptionQuantityActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateDatabaseSubscriptionQuantityArguments> context)
        {

            var arguments = context.Arguments;

            var subscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(arguments.Office365SubscriptionId);

            if(subscription == null)
                throw new NullReferenceException($"There is no subscription with office 365 subscription id {arguments.Office365SubscriptionId} in database");

            await _office365DbSubscriptionService.UpdateSubscriptionQuantity(arguments.Office365SubscriptionId,
                arguments.QuantityChange);

            return context.Completed(new UpdateDatabaseSubscriptionQuantityLog
            {
                Office365SubscriptionId = arguments.Office365SubscriptionId,
                QuantityChange = subscription.Quantity - arguments.QuantityChange
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdateDatabaseSubscriptionQuantityLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.UpdateSubscriptionQuantity(log.Office365SubscriptionId,
                    log.QuantityChange);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdateDatabaseSubscriptionQuantityActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}