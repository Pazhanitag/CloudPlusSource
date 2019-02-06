
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscription
{
    public class UpdateMultiDatabaseSubscriptionActivity : IUpdateMultiDatabaseSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateMultiDatabaseSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateMultiDatabaseSubscriptionArguments> context)
        {

            var arguments = context.Arguments;

            var subscriptions = await _office365DbSubscriptionService
                .GetSubscriptionsByProductIdentifiersAsync(arguments.Office365CustomerId, arguments.office365SubscriptionModels.Select(x => x.Office365Offer.CloudPlusProductIdentifier));

            if (subscriptions == null || subscriptions.Count == 0)
                throw new NullReferenceException($"Could not find subscription for customer {arguments.Office365CustomerId}, " +
                                                 $"identifier: { string.Join(",", arguments.office365SubscriptionModels.Select(x => x.Office365Offer.CloudPlusProductIdentifier))}");


            foreach (var subscription in subscriptions)
            {
                await _office365DbSubscriptionService.AddPartnerPlatformDataToSubscription(
               new Office365SubscriptionModel
               {
                   Office365CustomerId = arguments.Office365CustomerId,
                   Office365SubscriptionId = subscription.Office365SubscriptionId,
                   Office365OrderId = subscription.Office365OrderId,
                   Office365Offer = new Office365OfferModel
                   {
                       CloudPlusProductIdentifier = subscription.Office365Offer.CloudPlusProductIdentifier
                   }
               });
            }

            return context.CompletedWithVariables(new UpdateMultiDatabaseSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365OrderId = subscriptions.FirstOrDefault().Office365OrderId,
                office365SubscriptionModels = subscriptions

            }, new
            {
                Office365SubscriptionIds = string.Join(",", subscriptions.Select(x => x.Office365SubscriptionId))
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdateMultiDatabaseSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                foreach (var subscription in log.office365SubscriptionModels)
                {
                    await _office365DbSubscriptionService.AddPartnerPlatformDataToSubscription(
                   new Office365SubscriptionModel
                   {
                       Office365CustomerId = log.Office365CustomerId,
                       Office365SubscriptionId = subscription.Office365SubscriptionId,
                       Office365OrderId = log.Office365OrderId,
                       Office365Offer = new Office365OfferModel
                       {
                           CloudPlusProductIdentifier = subscription.Office365Offer.CloudPlusProductIdentifier
                       }
                   });
                }


            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdateMultiDatabaseSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
