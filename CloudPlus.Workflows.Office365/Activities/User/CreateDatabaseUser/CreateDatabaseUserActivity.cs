using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.User;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.CreateDatabaseUser
{
    public class CreateDatabaseUserActivity : ICreateDatabaseUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;

        public CreateDatabaseUserActivity(IOffice365DbUserService office365DbUserService)
        {
            _office365DbUserService = office365DbUserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbUserService.CreateOffice365DatabaseUserAsync(new Office365UserModel
            {
                UserPrincipalName = arguments.UserPrincipalName,
                Office365UserId = arguments.Office365UserId
            });

            return context.Completed(new CreateDatabaseUserLog
            {
                UserPrincipalName = arguments.UserPrincipalName
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateDatabaseUserLog> context)
        {
            try
            {
                var log = context.Log;

                await _office365DbUserService.DeleteOffice365DatabaseUserAsync(log.UserPrincipalName);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating CreateDatabaseUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
