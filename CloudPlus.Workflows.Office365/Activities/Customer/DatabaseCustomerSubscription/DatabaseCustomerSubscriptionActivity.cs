using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DatabaseCustomerSubscription
{
    public class DatabaseCustomerSubscriptionActivity : IDatabaseCustomerSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public DatabaseCustomerSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDatabaseCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;
            var quantity = 1;
            
                var subscription = await _office365DbSubscriptionService
                    .GetSubscriptionByProductIdentifierAsync(arguments.Office365CustomerId, arguments.CloudPlusProductIdentifier);

                if (subscription == null)
                {
                    await _office365DbSubscriptionService.CreateSubscription(new Office365SubscriptionModel
                    {
                        Office365CustomerId = arguments.Office365CustomerId,
                        Office365FriendlyName = "",
                        Quantity = quantity,
                        Office365Offer = new Office365OfferModel
                        {
                            CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier
                        }
                    });
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(subscription.Office365SubscriptionId))
                        throw new Exception($"There is no Office365SubscriptionId for Office365CustomerId {arguments.Office365CustomerId}");

                        await _office365DbSubscriptionService.IncreaseSubscriptionByProductIdentifierAsync(
                            arguments.Office365CustomerId, arguments.CloudPlusProductIdentifier);
                }
           
            return context.Completed(new DatabaseCustomerSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier,
                Quantity = quantity
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IDatabaseCustomerSubscriptionLog> context)
        {
            try
            {
                var logs = context.Log;

                var subscription = await _office365DbSubscriptionService
                    .GetSubscriptionByProductIdentifierAsync(logs.Office365CustomerId, logs.CloudPlusProductIdentifier);

                if (subscription.Quantity == 1)
                    await _office365DbSubscriptionService
                        .DeleteSubscriptionByProductIdentifierAsync(logs.Office365CustomerId, logs.CloudPlusProductIdentifier);
                else
                    await _office365DbSubscriptionService
                        .DecreaseSubscriptionByProductIdentifierAsync(logs.Office365CustomerId, logs.CloudPlusProductIdentifier);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating DatabaseCustomerSubscriptionActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
