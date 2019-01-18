using System.Threading.Tasks;

namespace CloudPlus.Services.Identity.Client
{
    public interface IClientService
    {
        Task<int> GetClientDbId(string clientId);
        Task AddRedirectUri(string uri, int clientDbId);
        Task UpdateRedirectUri(string oldUri, string newUri);
        Task RemoveRedirectUri(string uri, int clientDbId);
        Task AddPostLogoutRedirectUri(string uri, int clientDbId);
        Task UpdatePostLogoutRedirectUri(string oldUri, string newUri);
        Task RemovePostLogoutRedirectUri(string uri, int clientDbId);
    }
}
