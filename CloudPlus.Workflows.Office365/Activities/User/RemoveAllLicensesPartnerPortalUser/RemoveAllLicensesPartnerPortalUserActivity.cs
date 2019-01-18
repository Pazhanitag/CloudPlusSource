using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Services.Office365.License;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveAllLicensesPartnerPortalUser
{
    public class RemoveAllLicensesPartnerPortalUserActivity : IRemoveAllLicensesPartnerPortalUserActivity
    {
        private readonly IOffice365LicenseService _office365LicenseService;

        public RemoveAllLicensesPartnerPortalUserActivity(IOffice365LicenseService office365LicenseService)
        {
            _office365LicenseService = office365LicenseService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IRemoveAllLicensesPartnerPortalUserArguments> context)
        {
            var arguments = context.Arguments;

            var skuList = _office365LicenseService
                .GetAllUserLicenses(arguments.Office365CustomerId, arguments.Office365UserId)
                .Result.Select(l => l.Office365Offer.Sku).ToList();

            if (skuList.Any())
                await _office365LicenseService
                    .RemoveUserMultiLicenses(arguments.Office365CustomerId, arguments.Office365UserId, skuList);

            return context.Completed();
        }
    }
}
