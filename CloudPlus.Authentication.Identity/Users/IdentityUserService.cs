using IdentityServer3.AspNetIdentity;
using IdentityServer3.Core.Models;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CloudPlus.Entities.Identity;
using CloudPlus.Resources;
using CloudPlus.Authentication.Identity.Services;
using IdentityServer3.Core;
using System;
using CloudPlus.Enums.User;
using CloudPlus.Logging;
using IdentityServer3.Core.Extensions;

namespace CloudPlus.Authentication.Identity.Users
{
    public class IdentityUserService : AspNetIdentityUserService<User, int>
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IImpersonateUserService _impersonateUserService;

        public IdentityUserService(IdentityUserManager userMgr, IImpersonateUserService impersonateUserService, IConfigurationManager configurationManager) : base(userMgr)
        {
            _configurationManager = configurationManager;
            _impersonateUserService = impersonateUserService;
        }

        protected override Task<User> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId,
            IEnumerable<Claim> claims)
        {
            var claimsEnumerated = claims as Claim[] ?? claims.ToArray();
            var userName = claimsEnumerated.FirstOrDefault(p => p.Type == "email")?.Value;
            var firstName = claimsEnumerated.FirstOrDefault(p => p.Type == "given_name")?.Value;
            var lastName = claimsEnumerated.FirstOrDefault(p => p.Type == "family_name")?.Value;
            var user = new User { UserName = userName, Email = userName, FirstName = firstName, LastName = lastName };
            return Task.FromResult(user);
        }

        /// <summary>
        /// Impersonate user based on AcrValue flag
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            this.Log().Info("===> PreAuthenticateAsync");

            var acrValue = context.SignInMessage.AcrValues.FirstOrDefault(acr => acr.Contains("impersonate"));

            if (_impersonateUserService.IsImpersonateFlow(acrValue))
            {
                context.AuthenticateResult = await _impersonateUserService.ImpersonateUser(acrValue);
            }
            else
            {
                await base.PreAuthenticateAsync(context);
            }
        }

        /// <summary>
        /// Authenticate user for implicit flow
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext ctx)
        {
            this.Log().Info("===> AuthenticateLocalAsync");

            var username = ctx.UserName;
            var password = ctx.Password;
            var message = ctx.SignInMessage;

            ctx.AuthenticateResult = null;

            this.Log().Info($"userManager.SupportsUserPassword: {userManager.SupportsUserPassword}");

            if (userManager.SupportsUserPassword)
            {
                this.Log().Info("===> FindUserAsync");

                var user = await FindUserAsync(username);

                if (user != null)
                {
                    this.Log().Info($"UserStaus: {user.UserStatus}, username: {username}");

                    if (user.UserStatus != UserStatus.Active)
                    {
                        ctx.AuthenticateResult = new AuthenticateResult("Your account is not active. Please contact System Administrator.");
                        return;
                    }

                    try
                    {
                        this.Log().Info("Trying to connect to remote ActiveDirecotry server");
                        using (var pc = new PrincipalContext(ContextType.Domain, _configurationManager.GetByKey("ActiveDirectoryAddress")))
                        {
                            this.Log().Info($"Connected to ActiveDirectory server. Trying to authenticate user {username}");

                            if (pc.ValidateCredentials(username, password))
                            {
                               
                                this.Log().Info($"User {username} authenticated!");

                                var result = await PostAuthenticateLocalAsync(user, message);

                                if (result == null)
                                {
                                    this.Log().Info("result == null");

                                    var claims = await GetClaimsForAuthenticateResult(user, ctx);
                                    result = new AuthenticateResult(user.Id.ToString(),
                                        await GetDisplayNameForAccountAsync(user.Id), claims);
                                }

                                ctx.AuthenticateResult = result;
                            }
                            else if (userManager.SupportsUserLockout)
                            {
                                await userManager.AccessFailedAsync(user.Id);
                            }
                        }

                    }
                    catch (Exception exception)
                    {
                        this.Log().Error("Active Directory login error");
                        this.Log().Error($"{exception}");

                        ctx.AuthenticateResult = new AuthenticateResult("Unable to login");

                        throw;
                    }
                }
            }
        }

        protected async Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(User user, LocalAuthenticationContext ctx)
        {
            var claims = (await base.GetClaimsForAuthenticateResult(user)).ToList();
            
            claims.Add(new Claim("company_id", user.CompanyId.ToString()));

            if (ctx.AuthenticateResult != null && ctx.AuthenticateResult.User.HasClaim(c => c.Type == "parent_user_id"))
            {
                claims.Add(ctx.AuthenticateResult.User.Claims.FirstOrDefault(c => c.Type == "parent_user_id"));
            }

            return claims;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext ctx)
        {
            await base.GetProfileDataAsync(ctx);
            var issuedClaims = ctx.IssuedClaims.ToList();

            var subject = ctx.Subject;

            if (subject == null) throw new ArgumentNullException(nameof(ctx.Subject));

            var key = ConvertSubjectToKey(subject.GetSubjectId());

            var acct = await userManager.FindByIdAsync(key);

            if (acct == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            issuedClaims.AddRange(await GetClaimsFromAccount(acct, ctx));

            ctx.IssuedClaims = issuedClaims;
        }

        /// <summary>
        /// Set default claims for authenticated user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<Claim>> GetClaimsFromAccount(User user, ProfileDataRequestContext ctx)
        {
            var claims = (await base.GetClaimsFromAccount(user)).ToList();

            claims.Add(new Claim(Constants.ClaimTypes.Subject, user.Id.ToString()));
            claims.Add(new Claim(Constants.ClaimTypes.PreferredUserName, user.UserName));
            claims.Add(new Claim(Constants.ClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(Constants.ClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim("company_id", user.CompanyId.ToString()));
            claims.Add(new Claim(Constants.ClaimTypes.Picture, !string.IsNullOrWhiteSpace(user.ProfilePicture) ? $"{_configurationManager.GetByKey("ImageServerPath")}{user.ProfilePicture}" : ""));

            if (ctx.Subject.HasClaim(c => c.Type == "parent_user_id"))
            {
                claims.Add(ctx.Subject.Claims.FirstOrDefault(c => c.Type == "parent_user_id"));
            }

            return claims;
        }
    }
}
