using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__
{
    public class UpdateDatabaseSubscriptionActivity : IUpdateDatabaseSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateDatabaseSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateDatabaseSubscriptionArguments> context)
        {

            var arguments = context.Arguments;

            var subscription = await _office365DbSubscriptionService
                .GetSubscriptionByProductIdentifierAsync(arguments.Office365CustomerId, arguments.CloudPlusProductIdentifier);

            if (subscription == null)
                throw new NullReferenceException($"Could not find subscription for customer {arguments.Office365CustomerId}, " +
                                                 $"identifier: {arguments.CloudPlusProductIdentifier}");

            await _office365DbSubscriptionService.AddPartnerPlatformDataToSubscription(
                new Office365SubscriptionModel
                {
                    Office365CustomerId = arguments.Office365CustomerId,
                    Office365SubscriptionId = arguments.Office365SubscriptionId,
                    Office365OrderId = arguments.Office365OrderId,
                    Office365Offer = new Office365OfferModel
                    {
                        CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier
                    }
                });

            return context.CompletedWithVariables(new UpdateDatabaseSubscriptionLog
            {
                Office365CustomerId = subscription.Office365CustomerId,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Office365OrderId = subscription.Office365OrderId,
                CloudPlusProductIdentifier = subscription.Office365Offer?.CloudPlusProductIdentifier

            }, new
            {
                arguments.Office365SubscriptionId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IUpdateDatabaseSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;
                await _office365DbSubscriptionService.AddPartnerPlatformDataToSubscription(
                    new Office365SubscriptionModel
                    {
                        Office365CustomerId = log.Office365CustomerId,
                        Office365SubscriptionId = log.Office365SubscriptionId,
                        Office365OrderId = log.Office365OrderId,
                        Office365Offer = new Office365OfferModel
                        {
                            CloudPlusProductIdentifier = log.CloudPlusProductIdentifier
                        }
                    });

            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating UpdateDatabaseSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}