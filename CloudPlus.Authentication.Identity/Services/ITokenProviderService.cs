using System.Threading.Tasks;

namespace CloudPlus.Authentication.Identity.Services
{
    public interface ITokenProviderService
    {
        Task<bool> IsConfirmationTokenValid(string email, string token);
        Task<string> GenerateConfirmationToken(string userEmail);
    }
}
