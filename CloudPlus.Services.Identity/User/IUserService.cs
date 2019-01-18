using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Identity;
using CloudPlus.Models.Company;

namespace CloudPlus.Services.Identity.User
{
    public interface IUserService
    {
        UserModel Create(UserModel model);
        Task<UserModel> CreateAsync(UserModel model);
        void Delete(int id);
        Task DeleteAsync(int id);
        UserModel GetUser(int id);
        Task<UserModel> GetUserAsync(string email);
        UserModel GetUser(string email);
        Task<UserModel> GetUserAsync(int id);
        Task<UserModel> GetUserAsync(int id, int companyId);
        IEnumerable<UserModel> SearchUsers(string searchTerm, int companyId);
        UserModel Update(UserModel model);
        bool ResetSecurityStamp(string username);
        Task<UserModel> UpdateAsync(UserModel model);
        Task<bool> IsEmailValid(string email);
        IEnumerable<UserModel> GetUsers(int companyId);
        Task<bool> IsDisplayNameAvailable(string displayName, int companyId);
        Task GenerateImpersonateToken(int parentUserId, int impersonateUserId);
        Task<bool> IsImpersonateTokenValid(int parentUserId, int impersonateUserId);
        Task<UserModel> GetUserByAlternativeEmailAsync(string alternativeEmail);
        CompanyCountModel GetUserCountForCutomers(CompanyCountModel root);
        IEnumerable<UserModel> GetUsersByDomain(string domain, int comapanyId);
    }
}