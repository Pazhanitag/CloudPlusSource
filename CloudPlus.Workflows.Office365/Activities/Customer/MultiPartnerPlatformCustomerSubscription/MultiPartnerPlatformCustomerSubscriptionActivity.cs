using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription
{
    public class MultiPartnerPlatformCustomerSubscriptionActivity : IMultiPartnerPlatformCustomerSubscriptionActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public MultiPartnerPlatformCustomerSubscriptionActivity(
            IOffice365SubscriptionService office365SubscriptionService,
            IOffice356DbOfferService office356DbOfferService)
        {
            _office365SubscriptionService = office365SubscriptionService;
            _office356DbOfferService = office356DbOfferService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IMultiPartnerPlatformCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var filteredProductIdentifiers = arguments.ProductItems
                .Where(p => !p.Admin && !p.Delete && !p.RemoveLicenses && !p.KeepLicenses).ToList();

            var groupedProductIdentifiers = filteredProductIdentifiers
                .GroupBy(i => i.RecommendedProductItem.CloudPlusProductIdentifier).Select(c => c.Key);

            var offers = await _office356DbOfferService.GetOffice365OffersAsync();

            var createdSubscriptions = new List<Office365SubscriptionModel>();

            foreach (var identifier in groupedProductIdentifiers)
            {
                var offer = offers.FirstOrDefault(o => o.CloudPlusProductIdentifier == identifier);
                if (offer == null) continue;

                var quantity = filteredProductIdentifiers.Count(i =>
                    i.RecommendedProductItem.CloudPlusProductIdentifier == identifier);

                var subscription = await _office365SubscriptionService.CreatePartnerPlatformSubscriptionAsync(
                    arguments.Office365CustomerId, offer.Office365Id, quantity);

                if (subscription == null) continue;

                subscription.Office365Offer = new Office365OfferModel
                {
                    CloudPlusProductIdentifier = identifier,
                    Office365Id = offer.Office365Id
                };

                createdSubscriptions.Add(subscription);
            }

            return context.CompletedWithVariables(new MultiPartnerPlatformCustomerSubscriptionLog
            {
                Subscriptions = createdSubscriptions
            }, new
            {
                Subscriptions = createdSubscriptions
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IMultiPartnerPlatformCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                foreach (var subscription in logs.Subscriptions)
                {
                    await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(
                        subscription.Office365CustomerId, subscription.Office365Offer.Office365Id);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Compensate MultiPartnerPlatformCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
