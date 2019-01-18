using System.Web.Helpers;

namespace CloudPlus.Authentication.Mvc
{
    public class TokenVerificationService : ITokenVerificationService
    {
        public string GenerateAntiForgeryToken()
        {
            AntiForgery.GetTokens(null, out var cookieToken, out var formToken);

            return cookieToken + ":" + formToken;
        }

        public void ValidateAntiForgeryToken(string token)
        {
            var tokens = token.Split(':');

            if (tokens.Length != 2)
                return;

            var cookieToken = tokens[0].Trim();
            var formToken = tokens[1].Trim();

            AntiForgery.Validate(cookieToken, formToken);
        }
    }
}
