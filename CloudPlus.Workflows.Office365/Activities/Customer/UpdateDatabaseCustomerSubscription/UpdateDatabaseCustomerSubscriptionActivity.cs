using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseCustomerSubscription
{
    public class UpdateDatabaseCustomerSubscriptionActivity : IUpdateDatabaseCustomerSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateDatabaseCustomerSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateDatabaseCustomerSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            var subscription = await _office365DbSubscriptionService
                .GetSubscriptionByProductIdentifierAsync(arguments.Office365CustomerId, arguments.CloudPlusProductIdentifier);

            if (subscription == null)
                throw new NullReferenceException($"Could not find subscription for customer {arguments.Office365CustomerId}, " +
                                                 $"identifier: {arguments.CloudPlusProductIdentifier}");

            if (string.IsNullOrWhiteSpace(subscription.Office365SubscriptionId))
            {
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
            }

            return context.Completed();
        }
    }
}
