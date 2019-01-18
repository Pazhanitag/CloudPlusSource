using System;
using System.Net.Http;
using System.Threading.Tasks;
using CloudPlus.Extensions;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Resources;
using CloudPlus.Services.Identity.User;

namespace CloudPlus.Services.Identity.Password
{
    public class PasswordService : IPasswordService
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        public PasswordService(
            IHttpClientResolver httpClientResolver,
            IConfigurationManager configurationManager,
            IUserService userService)
        {
            _httpClient = httpClientResolver.GetIdentityServerHttpClient();
            _configurationManager = configurationManager;
            _userService = userService;
        }

        public async Task<string> GetPasswordResetLink(int userId, string userEmail)
        {
            var user = await _userService.GetUserAsync(userId);

            if (user == null)
                throw new Exception($"User not found Id: {userId}");

            if (!user.AlternativeEmail.IsNotNullAndEquals(userEmail) && !user.Email.IsNotNullAndEquals(userEmail))
                throw new Exception($"User email does not match userid. Id: {userId}, Email: {userEmail}");

            var passwordResetToken = await GetPasswordResetToken(userEmail);

            var resetLink =
                $"{_configurationManager.GetByKey("CloudPlus.PortalEndpoint")}{string.Format(_configurationManager.GetByKey("Portal.UpdatePassword"), user.Id, System.Web.HttpUtility.UrlEncode(passwordResetToken))}";
            return resetLink;
        }

        public async Task<bool> IsConfirmationTokenValid(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"{nameof(email)} cannot be null or empty");
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException($"{nameof(token)} cannot be null or empty");

            var response = await PostAsync(_configurationManager.GetByKey("IS.ValidatePasswordResetToken"), new
            {
                UserEmail = email,
                Token = token
            });

            return Convert.ToBoolean(response.Content.ReadAsAsync<HttpResponse>().Result.Result.ToString());
        }

        private async Task<string> GetPasswordResetToken(string userEmail)
        {
            var controllerEndpoint =
                string.Format(_configurationManager.GetByKey("IS.GetPasswordResetToken"), userEmail);

            var response = await GetAsync(controllerEndpoint + "?userEmail=" + userEmail);

            return Convert.ToString(response.Content.ReadAsAsync<HttpResponse>().Result.Result);
        }

        private async Task<HttpResponseMessage> GetAsync(string controllerActionUrl)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);

            var response = await _httpClient.GetAsync(url);

            return response;
        }

        private async Task<HttpResponseMessage> PostAsync<T>(string controllerActionUrl, T model)
        {
            var authenticationServiceEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var url = string.Concat(authenticationServiceEndpoint, controllerActionUrl);

            var response = await _httpClient.PostAsJsonAsync(url, model);

            return response;
        }
    }
}