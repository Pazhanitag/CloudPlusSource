using System.Threading.Tasks;
using CloudPlus.Models.Office365.License;

namespace CloudPlus.Services.Database.Office365.License
{
    public interface IOffice365DbLicenseService
    {
        Task<Office365LicenseModel> GetUserAssgnedLicenseAsync(string userPrincipalName);
        Task CreateOffice365UserLicense(string office365UserId, string office365OfferSku);
        Task<Office365LicenseModel> RemoveOffice365UserLicense(string userPrincipalName,
            string cloudPlusProductIdentifier);
    }
}
