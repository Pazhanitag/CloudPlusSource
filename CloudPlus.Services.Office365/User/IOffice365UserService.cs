using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.User;

namespace CloudPlus.Services.Office365.User
{
    public interface IOffice365UserService
    {
        Task<string> GetOffice365UserIdAsync(string userPrincipalName, string office365CustomerId);
        Task<List<Office365UserModel>> GetAllOffice365Users(string office365CustomerId);
        Task<Office365SdkUser> CreateOffice365UserAsync(Office365SdkUser user);
        Task DeleteOffice365UserAsync(string office365UserId, string office365CustomerId);
        Task RestoreOffice365UserAsync(string office365UserId, string office365CustomerId);
    }
}
