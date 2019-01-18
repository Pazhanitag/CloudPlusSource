using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.User;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles
{
    public class RemoveUserRolesActivity : IRemoveUserRolesActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public RemoveUserRolesActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveUserRolesArguments> context)
        {
            var arguments = context.Arguments;

            if (arguments.CurrentAssignedRoles.Any())
            {
                await _office365ApiService.RemoveUserRoles(new Office365UserRolesModel
                {
                    Office365CustomerId = arguments.Office365CustomerId,
                    UserPrincipalName = arguments.UserPrincipalName,
                    Roles = arguments.CurrentAssignedRoles
                });
            }

            return context.CompletedWithVariables(new RemoveUserRolesLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                UserPrincipalName = arguments.UserPrincipalName,
                CurrentAssignedRoles = arguments.CurrentAssignedRoles
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IRemoveUserRolesLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365ApiService.AssingUserRoles(new Office365UserRolesModel
                {
                    Office365CustomerId = logs.Office365CustomerId,
                    UserPrincipalName = logs.UserPrincipalName,
                    Roles = logs.CurrentAssignedRoles
                });
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensating RemoveUserRolesActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
