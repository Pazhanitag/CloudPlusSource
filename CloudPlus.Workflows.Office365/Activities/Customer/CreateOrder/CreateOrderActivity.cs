using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder
{
    public class CreateOrderActivity : ICreateOrderActivity
    {
        private readonly IOffice365SubscriptionService _office365SubscriptionService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public CreateOrderActivity(IOffice365SubscriptionService office365SubscriptionService, IOffice356DbOfferService office356DbOfferService)
        {
            _office365SubscriptionService = office365SubscriptionService;
            _office356DbOfferService = office356DbOfferService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateOrderArguments> context)
        {
            var arguments = context.Arguments;

            var offer = await _office356DbOfferService.GetOffice365OfferAsync(arguments.CloudPlusProductIdentifier);
            var subscription = await _office365SubscriptionService.CreatePartnerPlatformSubscriptionAsync(arguments.Office365CustomerId, offer.Office365Id, arguments.Quantity);

            return context.CompletedWithVariables(new CreateOrderLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365SubscriptionId = subscription.Office365SubscriptionId
            }, new
            {
                subscription.Office365SubscriptionId,
                subscription.Office365OrderId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateOrderLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365SubscriptionService.SuspendPartnerPlatformSubscriptionAsync(log.Office365CustomerId,
                                        log.Office365SubscriptionId);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating CreateOrderActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}