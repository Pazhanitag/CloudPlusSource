using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.SoftDeleteDatabaseUser
{
    public class SoftDeleteDatabaseUserActivity : ISoftDeleteDatabaseUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;

        public SoftDeleteDatabaseUserActivity(IOffice365DbUserService office365DbUserService)
        {
            _office365DbUserService = office365DbUserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ISoftDeleteDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbUserService.SoftDeleteOffice365DatabaseUserAsync(arguments.UserPrincipalName);

            return context.Completed(new SoftDeleteDatabaseUserLog
            {
                UserPrincipalName = arguments.UserPrincipalName
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ISoftDeleteDatabaseUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365DbUserService.ActivateOffice365DatabaseUserAsync(logs.UserPrincipalName);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensate SoftDeleteDatabaseUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
