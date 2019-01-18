using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Services.Database.Office365.License;

namespace CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToDatabaseUser
{
    public class AssignLicenseToDatabaseUserActivity : IAssignLicenseToDatabaseUserActivity
    {
        private readonly IOffice365DbLicenseService _office365DbLicenseService;

        public AssignLicenseToDatabaseUserActivity(IOffice365DbLicenseService office365DbLicenseService)
        {
            _office365DbLicenseService = office365DbLicenseService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAssignLicenseToDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            await _office365DbLicenseService.CreateOffice365UserLicense(arguments.UserPrincipalName, arguments.CloudPlusProductIdentifier);

            return context.Completed();
        }
    }
}
