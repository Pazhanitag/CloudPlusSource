using System;
using System.Data.Entity;
using System.Threading.Tasks;
using CloudPlus.Database.Authentication;
using IdentityServer3.EntityFramework.Entities;

namespace CloudPlus.Services.Identity.Client
{
    public class ClientService : IClientService
    {
        private readonly CloudPlusAuthDbContext _dbContext;

        public ClientService(CloudPlusAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetClientDbId(string clientId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (client == null)
                throw new Exception($"No Identity Server Client with ClientId {clientId}");

            return client.Id;
        }

        public async Task AddRedirectUri(string uri, int clientDbId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientDbId);
            if (client == null) throw new ArgumentException($"No Client with id: {clientDbId}");

            var clientRedirectUri = new ClientRedirectUri
            {
                Uri = uri.ToLower(),
                Client = client
            };

            _dbContext.ClientRedirectUri.Add(clientRedirectUri);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRedirectUri(string oldUri, string newUri)
        {
            var clientRedirectUri = await _dbContext.ClientRedirectUri.FirstOrDefaultAsync(r => r.Uri.Contains(oldUri.ToLower()));

            if (clientRedirectUri == null) return;

            clientRedirectUri.Uri = newUri.ToLower();

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRedirectUri(string uri, int clientDbId)
        {
            var clientRedirectUri = await _dbContext.ClientRedirectUri.FirstOrDefaultAsync(r => r.Uri == uri && r.Id == clientDbId);

            if (clientRedirectUri == null) return;

            _dbContext.ClientRedirectUri.Remove(clientRedirectUri);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddPostLogoutRedirectUri(string uri, int clientDbId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientDbId);
            if (client == null) throw new ArgumentException($"No Client with id: {clientDbId}");

            var clientPostLogoutRedirectUri = new ClientPostLogoutRedirectUri
            {
                Uri = uri.ToLower(),
                Client = client
            };

            _dbContext.ClientPostLogoutRedirectUris.Add(clientPostLogoutRedirectUri);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePostLogoutRedirectUri(string oldUri, string newUri)
        {
            var clientPostLogoutRedirectUri = await _dbContext.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(r => r.Uri.Contains(oldUri.ToLower()));

            if (clientPostLogoutRedirectUri == null) return;

            clientPostLogoutRedirectUri.Uri = newUri.ToLower();

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePostLogoutRedirectUri(string uri, int clientDbId)
        {
            var clientPostLogoutRedirectUri = await _dbContext.ClientPostLogoutRedirectUris.FirstOrDefaultAsync(r => r.Uri == uri && r.Id == clientDbId);

            if (clientPostLogoutRedirectUri == null) return;

            _dbContext.ClientPostLogoutRedirectUris.Remove(clientPostLogoutRedirectUri);
            await _dbContext.SaveChangesAsync();
        }
    }
}
