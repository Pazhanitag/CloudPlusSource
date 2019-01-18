using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Office365.License;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser
{
    public class RemoveLicensePartnerPortalUserActivity : IRemoveLicensePartnerPortalUserActivity
    {
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice365LicenseService _office365LicenseService;

        public RemoveLicensePartnerPortalUserActivity(
            IOffice365LicenseService office365LicenseService,
            IOffice365DbUserService office365DbUserService)
        {
            _office365LicenseService = office365LicenseService;
            _office365DbUserService = office365DbUserService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveLicensePartnerPortalUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365User = await _office365DbUserService.GetOffice365DatabaseUserWithLicensesAndOfferAsync(arguments.UserPrincipalName);

            var assignedLicense = office365User.Licenses
                .FirstOrDefault(l => l.Office365Offer.CloudPlusProductIdentifier == arguments.CloudPlusProductIdentifier);

            if (assignedLicense == null)
                throw new NullReferenceException($"Could not find User {arguments.UserPrincipalName} license!");

            await _office365LicenseService
                    .RemoveUserLicense(arguments.Office365CustomerId, office365User.Office365UserId, assignedLicense.Office365Offer.Sku);

            return context.Completed(new RemoveLicensePartnerPortalUserLog
            {
                Office365CustomerId = arguments.Office365CustomerId,
                Office365UserId = office365User.Office365UserId,
                Office365OfferSku = assignedLicense.Office365Offer.Sku
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IRemoveLicensePartnerPortalUserLog> context)
        {
            try
            {
                var logs = context.Log;

                await _office365LicenseService
                    .AssignUserLicense(logs.Office365CustomerId, logs.Office365UserId, logs.Office365OfferSku);
            }
            catch (Exception ex)
            {
                this.Log().Error("Compensate RemoveLicensePartnerPortalUserActivity failed!", ex);
            }

            return context.Compensated();
        }
    }
}
