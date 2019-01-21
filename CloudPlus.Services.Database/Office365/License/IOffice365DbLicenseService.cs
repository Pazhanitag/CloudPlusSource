using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.License;
using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Services.Database.Office365.License
{
    public interface IOffice365DbLicenseService
    {
        Task<List<Office365OfferModel>> GetUserAssgnedLicenseAsync(string userPrincipalName);
        Task CreateOffice365UserLicense(string office365UserId, string office365OfferSku);
        Task<Office365LicenseModel> RemoveOffice365UserLicense(string userPrincipalName,
            string cloudPlusProductIdentifier);
    }
}
