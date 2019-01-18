using System.Threading.Tasks;

namespace CloudPlus.Services.Identity.Password
{
    public interface IPasswordService
    {
        Task<string> GetPasswordResetLink(int userId, string userEmail);
        Task<bool> IsConfirmationTokenValid(string email, string token);
    }
}