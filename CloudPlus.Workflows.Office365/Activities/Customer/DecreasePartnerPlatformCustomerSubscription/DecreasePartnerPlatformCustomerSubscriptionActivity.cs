using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription
{
    public class DecreasePartnerPlatformCustomerSubscriptionActivity : IDecreasePartnerPlatformCustomerSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public DecreasePartnerPlatformCustomerSubscriptionActivity(
            IOffice365SubscriptionService office365SubscriptionService,
            IOffice365DbCustomerService office365DbCustomerService)
        {
            _office365SubscriptionService = office365SubscriptionService;
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDecreasePartnerPlatformCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(arguments.CompanyId);
            var subscription = office365Customer.Office365Subscriptions
                .FirstOrDefault(s => s.Office365Offer.CloudPlusProductIdentifier == arguments.CloudPlusProductIdentifier);

            if (subscription == null)
                throw new ArgumentException($"Could not find subscription for product identifier: {arguments.CloudPlusProductIdentifier}");

            if (subscription.Quantity == 1)
                await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(office365Customer.Office365CustomerId,
                    subscription.Office365SubscriptionId);
            else
                await _office365SubscriptionService.DecreasePartnerPlatformSubscriptionQuantityAsync(
                    office365Customer.Office365CustomerId, subscription.Office365SubscriptionId);

            return context.CompletedWithVariables(new DecreasePartnerPlatformCustomerSubscriptionLog
            {
                Office365CustomerId = office365Customer.Office365CustomerId,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Quantity = subscription.Quantity
            }, new
            {
                office365Customer.Office365CustomerId,
                subscription.Office365SubscriptionId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IDecreasePartnerPlatformCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                if (logs.Quantity == 1)
                {
                    // Activate suspended subscription
                    await _office365SubscriptionService.ActivateSuspendedPartnerPlatformSubscriptionAsync(
                        logs.Office365CustomerId, logs.Office365SubscriptionId);
                }
                else
                {
                    // Update subscription
                    await _office365SubscriptionService.IncreasePartnerPlatformSubscriptionQuantityAsync(
                        logs.Office365CustomerId, logs.Office365SubscriptionId);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DecreasePartnerPlatformCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
