using CloudPlus.Services.Database.Office365.Subscription;
using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateMultiDatabaseSubscriptionState
{
    public class UpdateMultiDatabaseSubscriptionStateActivity: IUpdateMultiDatabaseSubscriptionStateActivity
    {
        private readonly IOffice365DbSubscriptionService _office365DbSubscriptionService;

        public UpdateMultiDatabaseSubscriptionStateActivity(IOffice365DbSubscriptionService office365DbSubscriptionService)
        {
            _office365DbSubscriptionService = office365DbSubscriptionService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IUpdateMultiDatabaseSubscriptionStateArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbSubscriptionService.UpdateMultiDatabseSubscriptionState(arguments.SubscriptionState, arguments.Office365SubscriptionIds);

            return context.Completed();
        }
    }
}
