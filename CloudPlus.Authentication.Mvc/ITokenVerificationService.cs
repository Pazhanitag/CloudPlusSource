
namespace CloudPlus.Authentication.Mvc
{
    public interface ITokenVerificationService
    {
        string GenerateAntiForgeryToken();
        void ValidateAntiForgeryToken(string token);
    }
}
