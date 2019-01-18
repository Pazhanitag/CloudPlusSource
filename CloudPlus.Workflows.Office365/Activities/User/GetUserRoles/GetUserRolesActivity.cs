using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Models.Office365.User;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.Office365.Customer;

namespace CloudPlus.Workflows.Office365.Activities.User.GetUserRoles
{
    public class GetUserRolesActivity : IGetUserRolesActivity
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365ApiService _office365ApiService;

        public GetUserRolesActivity(
            IOffice365DbCustomerService office365DbCustomerService, 
            IOffice365ApiService office365ApiService)
        {
            _office365DbCustomerService = office365DbCustomerService;
            _office365ApiService = office365ApiService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IGetUserRolesArguments> context)
        {
            var arguments = context.Arguments;

            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerAsync(arguments.CompanyId);

            var roles = await _office365ApiService.GetUserRoles(new Office365UserRolesModel
            {
                Office365CustomerId = office365Customer.Office365CustomerId,
                UserPrincipalName = arguments.UserPrincipalName
            });

            return context.CompletedWithVariables(new
            {
                office365Customer.Office365CustomerId,
                CurrentAssignedRoles = roles
            });
        }
    }
}
