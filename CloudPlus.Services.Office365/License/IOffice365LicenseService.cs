using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.License;

namespace CloudPlus.Services.Office365.License
{
    public interface IOffice365LicenseService
    {
        Task<List<Office365LicenseModel>> GetAllUserLicenses(string office365CustomerId, string office365UserId);
        Task AssignUserLicense(string office365CustomerId, string office365UserId, string office365OfferSku);
        Task RemoveUserLicense(string office365CustomerId, string office365UserId, string office365ProductSku);
        Task RemoveUserMultiLicenses(string office365CustomerId, string office365UserId,
            List<string> office365ProductSkuList);
    }
}
