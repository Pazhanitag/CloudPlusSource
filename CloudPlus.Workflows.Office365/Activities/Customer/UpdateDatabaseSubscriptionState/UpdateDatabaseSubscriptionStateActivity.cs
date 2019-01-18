using System.Threading.Tasks;
using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionState
{

    public class UpdateDatabaseSubscriptionStateActivity : IUpdateDatabaseSubscriptionStateActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateDatabaseSubscriptionStateActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateDatabaseSubscriptionStateArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbSubscriptionService.UpdateDatabseSubscriptionState(arguments.SubscriptionState, arguments.Office365SubscriptionId);

            return context.Completed();
        }
    }
}