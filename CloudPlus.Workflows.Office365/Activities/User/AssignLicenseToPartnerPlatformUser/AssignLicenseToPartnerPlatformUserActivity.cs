using System;
using System.Threading.Tasks;
using CloudPlus.Services.Database.Office365.Offer;
using MassTransit.Courier;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Office365.License;

namespace CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser
{
    public class AssignLicenseToPartnerPlatformUserActivity : IAssignLicenseToPartnerPlatformUserActivity
    {
        private readonly IOffice365LicenseService _office365LicenseService;
        private readonly IOffice365DbUserService _office365DbUserService;
        private readonly IOffice356DbOfferService _office356DbOfferService;

        public AssignLicenseToPartnerPlatformUserActivity(
            IOffice365LicenseService office365LicenseService,
            IOffice365DbUserService office365DbUserService, 
            IOffice356DbOfferService office356DbOfferService)
        {
            _office365LicenseService = office365LicenseService;
            _office365DbUserService = office365DbUserService;
            _office356DbOfferService = office356DbOfferService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAssignLicenseToPartnerPlatformUserArguments> context)
        {
            var arguments = context.Arguments;

            var office365User = await _office365DbUserService.GetOffice365DatabaseUserAsync(arguments.UserPrincipalName);

            if (office365User == null)
                throw new Exception($"No Office365 user with upn {arguments.UserPrincipalName}");

            var office365Offer = await _office356DbOfferService
                .GetOffice365OfferAsync(arguments.CloudPlusProductIdentifier);

            await _office365LicenseService
                .AssignUserLicense(arguments.Office365CustomerId, office365User.Office365UserId, office365Offer.Sku);

            return context.Completed();
        }
    }
}
