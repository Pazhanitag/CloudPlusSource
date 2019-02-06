using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Order;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;


namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems
{
    public class CreateOrderWithMultiItemsActivity : ICreateOrderWithMultiItemsActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public CreateOrderWithMultiItemsActivity(IOffice365SubscriptionService office365SubscriptionService, IOffice356DbOfferService office356DbOfferService)
        {
            _office365SubscriptionService = office365SubscriptionService;
            _office356DbOfferService = office356DbOfferService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateOrderWithMultiItemsArguments> context)
        {
            var arguments = context.Arguments;

            var offers = await _office356DbOfferService.GetOffice365OffersAsync();
            var orderwithDetails = new Office365OrderWithDetailsModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365OrderDetailsModels = offers.Where(
                    o => arguments.CloudPlusProductIdentifiers.Any(x => x.Key == o.CloudPlusProductIdentifier))
                .Select(x => new Office365OrderDetailsModel() { FriendlyName = x.OfferName, Office365OfferId = x.Office365Id, Quantity = arguments.CloudPlusProductIdentifiers.Where(c => c.Key == x.CloudPlusProductIdentifier).FirstOrDefault().Value }).ToList()
            };
            var subscriptions = await _office365SubscriptionService.CreateMultiPartnerPlatformSubscriptionsAsync(orderwithDetails);

            return context.CompletedWithVariables(new CreateOrderWithMultiItemsLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365Subscriptions = subscriptions.office365SubscriptionModels.Select(x => x.Office365SubscriptionId).ToList()
            }, new
            {
                Office365Subscriptions = subscriptions.office365SubscriptionModels.Select(x => x.Office365SubscriptionId).ToList(),
                Office365OrderId = subscriptions.OrderId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateOrderWithMultiItemsLog> context)
        {
            try
            {
                var log = context.Log;
                foreach (var Office365SubscriptionId in log.Office365Subscriptions)
                {
                    await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(log.Office365CustomerId,
                                            Office365SubscriptionId);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating CreateOrderActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
