using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription
{
    public class DecreaseDatabaseCustomerSubscriptionActivity : IDecreaseDatabaseCustomerSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public DecreaseDatabaseCustomerSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDecreaseDatabaseCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var subscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(arguments.Office365SubscriptionId);

            if (subscription == null)
                throw new ArgumentException($"Could not find subscription for Office 365 Subscription Id: {arguments.Office365SubscriptionId}");

            if (subscription.Quantity == 1)
                await _office365DbSubscriptionService.DeleteSubscription(arguments.Office365SubscriptionId);
            else
                await _office365DbSubscriptionService.DecreaseSubscription(arguments.Office365SubscriptionId);

            return context.Completed(new DecreaseDatabaseCustomerSubscriptionLog
            {
                Office365CustomerId = subscription.Office365CustomerId,
                CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Office365OrderId = subscription.Office365OrderId,
                Office365FriendlyName = subscription.Office365FriendlyName
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IDecreaseDatabaseCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                var subscription = await _office365DbSubscriptionService.GetSubscriptionAsyc(logs.Office365SubscriptionId);

                if (subscription == null)
                {
                    await _office365DbSubscriptionService.CreateSubscription(new Office365SubscriptionModel
                    {
                        Office365CustomerId = logs.Office365CustomerId,
                        Office365SubscriptionId = logs.Office365SubscriptionId,
                        Office365OrderId = logs.Office365OrderId,
                        Office365FriendlyName = logs.Office365FriendlyName,
                        Quantity = 1,
                        Office365Offer = new Office365OfferModel
                        {
                            CloudPlusProductIdentifier = logs.CloudPlusProductIdentifier
                        }
                    });
                }
                else
                {
                    await _office365DbSubscriptionService.IncreaseSubscription(logs.Office365SubscriptionId);
                }
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DecreaseDatabaseCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
