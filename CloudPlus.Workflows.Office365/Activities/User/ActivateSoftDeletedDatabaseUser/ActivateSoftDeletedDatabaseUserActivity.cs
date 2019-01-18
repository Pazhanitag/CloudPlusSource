using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.User;

namespace CloudPlus.Workflows.Office365.Activities.User.ActivateSoftDeletedDatabaseUser
{
    public class ActivateSoftDeletedDatabaseUserActivity : IActivateSoftDeletedDatabaseUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;

        public ActivateSoftDeletedDatabaseUserActivity(IOffice365DbUserService office365DbUserService)
        {
            _office365DbUserService = office365DbUserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IActivateSoftDeletedDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365User = await _office365DbUserService.ActivateOffice365DatabaseUserAsync(arguments.UserPrincipalName);

            return context.CompletedWithVariables(new ActivateSoftDeletedDatabaseUserLog
            {
                UserPrincipalName = arguments.UserPrincipalName
            }, new
            {
                office365User.Licenses.FirstOrDefault()?.Office365Offer.CloudPlusProductIdentifier
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IActivateSoftDeletedDatabaseUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365DbUserService.SoftDeleteOffice365DatabaseUserAsync(logs.UserPrincipalName);
            }
            catch (Exception ex)
            {
                this.Log().Error("Componsate ActivateSoftDeletedDatabaseUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
