using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseSubscription
{
    public class CreateDatabaseSubscriptionActivity : ICreateDatabaseSubscriptionActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public CreateDatabaseSubscriptionActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseSubscriptionArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbSubscriptionService.CreateSubscription(new Office365SubscriptionModel
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365FriendlyName = "",
                Quantity = arguments.Quantity,
                SubscriptionState = Office365SubscriptionState.OperationInProgress,
                Office365Offer = new Office365OfferModel
                {
                    CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier
                }
            });

            return context.Completed(new CreateDatabaseSubscriptionLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                CloudPlusProductIdentifier = arguments.CloudPlusProductIdentifier
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateDatabaseSubscriptionLog> context)
        {
            try
            {
                var log = context.Log;

                await _office365DbSubscriptionService.DeleteSubscriptionByProductIdentifierAsync(log.Office365CustomerId,
                    log.CloudPlusProductIdentifier);

            }
            catch (System.Exception ex)
            {
                this.Log().Error("Compensating CreateDatabaseSubscriptionActivity failed!", ex);
            }
            return context.Compensated();
        }
    }
}