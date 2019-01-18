using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Web.Security;
using CloudPlus.Authentication.Identity.TokenProviders;
using CloudPlus.Database.Authentication;
using CloudPlus.Entities.Identity;
using CloudPlus.Resources;

namespace CloudPlus.Authentication.Identity.Users
{
    public class IdentityUserManager : UserManager<User, int>
    {
        public IdentityUserManager(UserStore store, IConfigurationManager configurationManager) : base(store)
        {
            UserValidator = new UserValidator<User, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            SetDefaultConfiguration(configurationManager);
        }

        public IdentityUserManager(IOwinContext context, IConfigurationManager configurationManager)
            : base(new UserStore(context.Get<CloudPlusAuthDbContext>()))
        {
            SetDefaultConfiguration(configurationManager);
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return Task.FromResult(Users.FirstOrDefault(u => u.UserName == userName && !u.IsDeleted));
        }

        private void SetDefaultConfiguration(IConfigurationManager configurationManager)
        {
            int.TryParse(configurationManager.GetByKey("DefaultConfirmationTokenTimeSpanInHours"), out var confirmationTokenTimeSpan);

            var dataProtectionProvider = new MachineKeyProtectionProvider();

            UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("EmailConfirmation", "ConfirmationToken")) {
                TokenLifespan = TimeSpan.FromHours(confirmationTokenTimeSpan)
            };

            bool.TryParse(configurationManager.GetByKey("UserLockoutEnabledByDefault"), out var userLockoutEnabled);

            if (!userLockoutEnabled) return;
            int.TryParse(configurationManager.GetByKey("MaxFailedAccessAttemptsBeforeLockout"), out var maxFailedAccessAttempts);

            int.TryParse(configurationManager.GetByKey("DefaultAccountLockoutTimeSpanInHours"), out var accountLockoutTimeSpan);

            MaxFailedAccessAttemptsBeforeLockout = maxFailedAccessAttempts;
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromHours(accountLockoutTimeSpan);
        }
    }
}
