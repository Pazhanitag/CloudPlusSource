using CloudPlus.Authentication.Identity.Users;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using CloudPlus.Database.Authentication;

namespace CloudPlus.Authentication.Identity.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IdentityUserManager _identityUserManager;
        private readonly CloudPlusAuthDbContext _dbContext;
        public TokenProviderService(IOwinContext request)
        {
            _identityUserManager = request.GetUserManager<IdentityUserManager>();
            _dbContext = request.Get<CloudPlusAuthDbContext>();
        }

        public TokenProviderService(IdentityUserManager identityUserManager, CloudPlusAuthDbContext dbContext)
        {
            _identityUserManager = identityUserManager;
            _dbContext = dbContext;
        }

        public async Task<string> GenerateConfirmationToken(string userEmail)
        {
            if(string.IsNullOrWhiteSpace(userEmail))
                throw new ArgumentNullException(nameof(userEmail));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Email.Equals(userEmail) || u.AlternativeEmail.Equals(userEmail));

            if(user == null)
                throw new Exception($"Could not find user ${userEmail}");

            return await _identityUserManager.UserTokenProvider.GenerateAsync("ConfirmationToken",
                _identityUserManager, user);
        }

        public async Task<bool> IsConfirmationTokenValid(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"{nameof(email)} cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException($"{nameof(token)} cannot be null or empty.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return await _identityUserManager.UserTokenProvider.ValidateAsync("ConfirmationToken",
                token, _identityUserManager, user);
        }
    }
}