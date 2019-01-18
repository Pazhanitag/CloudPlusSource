using System.Net.Http;

namespace CloudPlus.Infrastructure.Http
{
    public interface IHttpClientResolver
    {
        HttpClient GetIdentityServerHttpClient();
        HttpClient GetActiveDirectoryHttpClient();
        HttpClient GetHttpClient();
        HttpClient GetOffice365ApiHttpClient();
    }
}