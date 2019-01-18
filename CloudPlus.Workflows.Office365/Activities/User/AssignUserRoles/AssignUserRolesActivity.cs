using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.User;
using CloudPlus.Services.Database.Office365.Api;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles
{
    public class AssignUserRolesActivity : IAssignUserRolesActivity
    {
        private readonly IOffice365ApiService _office365ApiService;

        public AssignUserRolesActivity(IOffice365ApiService office365ApiService)
        {
            _office365ApiService = office365ApiService;
        }
        public async Task<ExecutionResult> Execute(ExecuteContext<IAssignUserRolesArguments> context)
        {
            var args = context.Arguments;

            if (args.UserRoles.Any())
            {
                await _office365ApiService.AssingUserRoles(new Office365UserRolesModel
                {
                    Office365CustomerId = args.Office365CustomerId,
                    UserPrincipalName = args.UserPrincipalName,
                    Roles = args.UserRoles
                });
            }

            return context.Completed();
        }
    }
}