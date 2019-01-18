using System;
using System.Net.Http;
using CloudPlus.Resources;
using IdentityModel.Client;
using System.Net.Http.Headers;

namespace CloudPlus.Infrastructure.Http
{
    public class HttpClientResolver : IHttpClientResolver
    {
        private readonly IConfigurationManager _configurationManager;

        public HttpClientResolver(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public HttpClient GetIdentityServerHttpClient()
        {
            var identityServerEndpoint = _configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint");
            var tokenEndpoint = identityServerEndpoint +
                                _configurationManager.GetByKey("CloudPlus.IdentityServerTokenEndpoint");

            var clientId = _configurationManager.GetByKey("CloudPlus.PortalClientId");
            var clientSecret = _configurationManager.GetByKey("CloudPlus.PortalClientSecret");

            if (string.IsNullOrWhiteSpace(identityServerEndpoint) || string.IsNullOrWhiteSpace(tokenEndpoint) ||
                string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("Invalid client configuration");

            var tokenClient = new TokenClient(tokenEndpoint, clientId, clientSecret);

            var tokenClientResponse = tokenClient
                .RequestClientCredentialsAsync(_configurationManager.GetByKey("CloudPlus.RequiredScopes"))
                .Result;

            var httpClient = new HttpClient();

            httpClient.SetBearerToken(tokenClientResponse.AccessToken);
            httpClient.BaseAddress = new Uri(_configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint"));

            return httpClient;
        }

        public HttpClient GetActiveDirectoryHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configurationManager.GetByKey("CloudPlus.ActiveDirectoryEndpoint"))
            };

            return httpClient;
        }

        public HttpClient GetHttpClient()
        {
            return new HttpClient();
        }

        public HttpClient GetOffice365ApiHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configurationManager.GetByKey("CloudPlus.Office365ApiEndpoint"))
            };

            return httpClient;
        }
    }
}