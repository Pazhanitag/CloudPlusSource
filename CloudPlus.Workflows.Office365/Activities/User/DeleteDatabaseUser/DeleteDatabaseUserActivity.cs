using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.DeleteDatabaseUser
{
    public class DeleteDatabaseUserActivity : IDeleteDatabaseUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;

        public DeleteDatabaseUserActivity(IOffice365DbUserService office365DbUserService)
        {
            _office365DbUserService = office365DbUserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDeleteDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbUserService.DeleteOffice365DatabaseUserAsync(arguments.UserPrincipalName);

            return context.Completed();
        }
    }
}
