using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.License;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicenseDatabaseUser
{
    public class RemoveLicenseDatabaseUserActivity : IRemoveLicenseDatabaseUserActivity
    {
        private readonly IOffice365DbLicenseService _office365DbLicenseService;

        public RemoveLicenseDatabaseUserActivity(IOffice365DbLicenseService office365DbLicenseService)
        {
            _office365DbLicenseService = office365DbLicenseService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveLicenseDatabaseUserArguments> context)
        {
            var arguments = context.Arguments;

            var removedLicense = await _office365DbLicenseService
                .RemoveOffice365UserLicense(arguments.UserPrincipalName, arguments.CloudPlusProductIdentifier);

            return context.Completed(new RemoveLicenseDatabaseUserLog
            {
                UserPrincipalName = arguments.UserPrincipalName,
                CloudPlusProductIdentifier = removedLicense.Office365Offer.CloudPlusProductIdentifier
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IRemoveLicenseDatabaseUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365DbLicenseService.CreateOffice365UserLicense(logs.UserPrincipalName, logs.CloudPlusProductIdentifier);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensate RemoveLicenseDatabaseUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
