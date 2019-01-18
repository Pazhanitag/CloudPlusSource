using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Identity;
using CloudPlus.Models.Office365.User;

namespace CloudPlus.Services.Database.Office365.User
{
    public interface IOffice365DbUserService
    {
        Task<Office365UserModel> GetOffice365DatabaseUserAsync(string userPrincipalName);
        Task<Office365UserModel> GetOffice365DatabaseUserWithLicensesAndOfferAsync(string userPrincipalName);
        Task<List<Office365UserModel>> GetAllCustomerUsersWithLicensesAndOfferAsync(int customerId);
        Task<Office365UserModel> CreateOffice365DatabaseUserAsync(Office365UserModel model);
        Task<Office365UserModel> ActivateOffice365DatabaseUserAsync(string userPrincipalName);
        Task DeleteOffice365DatabaseUserAsync(string userPrincipalName);
        Task SoftDeleteOffice365DatabaseUserAsync(string userPrincipalName);
        Task<Office365UserState?> GetOffice365UserState(string userPrincipalName);
        Task<IEnumerable<UserModel>> GetUsersByDomainAsync(string domain, string searchTerm, int companyId);
    }
}
