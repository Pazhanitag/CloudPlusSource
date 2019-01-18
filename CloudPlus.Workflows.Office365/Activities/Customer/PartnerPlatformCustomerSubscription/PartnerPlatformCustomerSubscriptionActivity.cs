using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.PartnerPlatformCustomerSubscription
{
    public class PartnerPlatformCustomerSubscriptionActivity : IPartnerPlatformCustomerSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public PartnerPlatformCustomerSubscriptionActivity(
            IOffice365SubscriptionService office365SubscriptionService,
            IOffice365DbCustomerService office365DbCustomerService,
            IOffice356DbOfferService office356DbOfferService)
        {
            _office365SubscriptionService = office365SubscriptionService;
            _office365DbCustomerService = office365DbCustomerService;
            _office356DbOfferService = office356DbOfferService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IPartnerPlatformCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(arguments.CompanyId);
            var subscription = office365Customer.Office365Subscriptions
                .FirstOrDefault(s => s.Office365Offer.CloudPlusProductIdentifier == arguments.CloudPlusProductIdentifier);

            if (subscription == null || string.IsNullOrWhiteSpace(subscription.Office365SubscriptionId))
            {
                // Create or Activate suspended subscription
                var offer = await _office356DbOfferService.GetOffice365OfferAsync(arguments.CloudPlusProductIdentifier);
                var partnerPortalSubscription = await _office365SubscriptionService
                    .GetPartnerPlatformSubscriptionByOfferAsync(office365Customer.Office365CustomerId, offer.Office365Id);

                if (partnerPortalSubscription == null)
                    subscription = await _office365SubscriptionService.CreatePartnerPlatformSubscriptionAsync(
                        office365Customer.Office365CustomerId, offer.Office365Id);
                else
                    subscription = await _office365SubscriptionService.ActivateSuspendedPartnerPlatformSubscriptionAsync(
                        office365Customer.Office365CustomerId, partnerPortalSubscription.Office365SubscriptionId);
            }
            else
            {
                // Update subscription
                await _office365SubscriptionService.IncreasePartnerPlatformSubscriptionQuantityAsync(
                    office365Customer.Office365CustomerId, subscription.Office365SubscriptionId);
            }

            return context.CompletedWithVariables(new PartnerPlatformCustomerSubscriptionLog
            {
                Office365CustomerId = office365Customer.Office365CustomerId,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Quantity = subscription.Quantity
            }, new
            {
                subscription.Office365SubscriptionId,
                subscription.Office365OrderId,
                subscription.Office365FriendlyName,
                subscription.Quantity,
                office365Customer.Office365CustomerId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IPartnerPlatformCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                if (logs.Quantity == 1)
                    await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(logs.Office365CustomerId,
                        logs.Office365SubscriptionId);
                else
                    await _office365SubscriptionService.DecreasePartnerPlatformSubscriptionQuantityAsync(
                        logs.Office365CustomerId, logs.Office365SubscriptionId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating PartnerPlatformCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
