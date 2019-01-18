using CloudPlus.Resources;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Identity.Permission;
using IUserService = CloudPlus.Services.Identity.User.IUserService;

namespace CloudPlus.Authentication.Identity.Services
{
    public class ImpersonateUserService : IImpersonateUserService
    {
        private readonly OwinContext _owinContext;
        private readonly IConfigurationManager _configurationManager;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        public ImpersonateUserService(
            OwinEnvironmentService owinEnvironmentService, 
            IConfigurationManager configurationManager,
            IPermissionService permissionService,
            IUserService userService)
        {
            _owinContext = new OwinContext(owinEnvironmentService.Environment);
            _configurationManager = configurationManager;
            _userService = userService;
            _permissionService = permissionService;
        }

        public bool IsImpersonateFlow(string acrValue)
        {
            this.Log().Info("===> IsImpersonateFlow");

            return IsImpersonateUserFlow(acrValue) || IsRevertImpersonateUserFlow();
        }

        /// <summary>
        /// Impersonate child user or revert back to parent user.
        /// </summary>
        /// <param name="acrValue"></param>
        /// <returns></returns>
        public async Task<AuthenticateResult> ImpersonateUser(string acrValue)
        {
            this.Log().Info("===> ImpersonateUser");
            this.Log().Info($"acrValue: {acrValue}");

            if (!_owinContext.Authentication.User.Identity.IsAuthenticated)
            {
                this.Log().Error("ImpersonateUser");

                return new AuthenticateResult("Impersonate user error.");
            }

            if (IsImpersonateUserFlow(acrValue))
            {
                int.TryParse(acrValue.Split(':')[1], out var impersonatedUserId);

                return await ImpersonateChildUser(impersonatedUserId);
            }

            if (!IsRevertImpersonateUserFlow())
            {
                return new AuthenticateResult("Impersonate user error.");
            }

            var parentUserId = GetParentUserId();

            this.Log().Info("<=== ImpersonateUser");

            return await ImpersonateParentUser(parentUserId);
        }

        /// <summary>
        /// Impersonate child user only in case if acrValue from the request contain the flag "impersonate"
        /// </summary>
        /// <param name="acrValue"></param>
        /// <returns></returns>
        private bool IsImpersonateUserFlow(string acrValue)
        {
            this.Log().Info("===> IsImpersonateUserFlow");

            if (string.IsNullOrEmpty(acrValue))
            {
                return false;
            }

            var impersonateFlowFlag = acrValue.Split(':')[0];
            var impersonateUserIdValue = acrValue.Split(':')[1];

            var acrValueIsValid = !string.IsNullOrEmpty(impersonateFlowFlag) &&
                                  !string.IsNullOrEmpty(impersonateUserIdValue) &&
                                  impersonateFlowFlag == _configurationManager.GetByKey("ImpersonateFlowIdentifier");

            return acrValueIsValid;
        }

        /// <summary>
        /// Impersonate parent user (revert flow) only in case the child user has the claim "ParentUserId"
        /// </summary>
        /// <returns></returns>
        private bool IsRevertImpersonateUserFlow()
        {
            this.Log().Info("===> IsRevertImpersonateUserFlow");

            var authenticatedUserImpersonateClaim = GetAuthenticatedUserImpersonateClaim();

            return authenticatedUserImpersonateClaim != null;
        }

        private async Task<AuthenticateResult> ImpersonateParentUser(int parentUserId)
        {
            this.Log().Info("===> ImpersonateParentUser");

            if (parentUserId <= 0)
            {
                return new AuthenticateResult("Invalid attempt to impersonate user");
            }

            if (!UserAllowedToImpersonate(parentUserId))
            {
                return new AuthenticateResult("Invalid attempt to impersonate user");
            }

            var user = await _userService.GetUserAsync(parentUserId);

            if (user != null)
            {
                return new AuthenticateResult(
                    user.Id.ToString(),
                    user.Email,
                    new List<Claim> {
                        new Claim("company_id", user.CompanyId.ToString())
                        });
            }

            return new AuthenticateResult("Invalid attempt to impersonate user");
        }

        private async Task<AuthenticateResult> ImpersonateChildUser(int userId)
        {
            this.Log().Info("===> ImpersonateChildUser");

            if (userId <= 0)
            {
                return new AuthenticateResult("Invalid impersonate user");
            }

            var authenticatedUserId = GetAuthenticatedUserId();

            if (!UserAllowedToImpersonate(authenticatedUserId))
            {
                return new AuthenticateResult("Invalid attempt to impersonate user");
            }

            var impersonatedUser = await _userService.GetUserAsync(userId);

            if (impersonatedUser == null)
            {
                return new AuthenticateResult("Invalid attempt to impersonate user");
            }

            if (!await IsImpersonateTokenValid(authenticatedUserId, impersonatedUser.Id))
            {
                return new AuthenticateResult("Invalid attempt to impersonate user");
            }

            var authenticatedUser = await _userService.GetUserAsync(authenticatedUserId);

            var parentUserId = GetParentUserId();

            return new AuthenticateResult(
                impersonatedUser.Id.ToString(),
                impersonatedUser.Email,
                new List<Claim> {
                    new Claim("parent_user_id", parentUserId != 0 ? parentUserId.ToString() : authenticatedUser.Id.ToString()),
                    new Claim(Constants.ClaimTypes.GivenName, impersonatedUser.FirstName),
                    new Claim(Constants.ClaimTypes.FamilyName, impersonatedUser.LastName),
                    new Claim(Constants.ClaimTypes.Email, impersonatedUser.Email),
                    new Claim("company_id", impersonatedUser.CompanyId.ToString())
                });
        }

        /// <summary>
        /// Validate impersonate token from the database based on parent and impersonate user
        /// </summary>
        /// <param name="parentUserId"></param>
        /// <param name="impersonatedUserId"></param>
        /// <returns></returns>
        private async Task<bool> IsImpersonateTokenValid(int parentUserId, int impersonatedUserId)
        {
            this.Log().Info("===> IsImpersonateTokenValid");

            var impersonateTokenValid = await _userService.IsImpersonateTokenValid(parentUserId, impersonatedUserId);

            return impersonateTokenValid;
        }

        private bool UserAllowedToImpersonate(int userId)
        {
            this.Log().Info("===> UserAllowedToImpersonate");

            if (userId <= 0)
            {
                return false;
            }

            var userPermissions = _permissionService.GetUserPermissions(userId);

            var allowedToImpersonate = userPermissions.Any(permission => permission.Name == "EditUser"
                || permission.Name == "EditAccounts");

            return allowedToImpersonate;
        }

        /// <summary>
        /// Get user id for the currently authenticated user from the claim
        /// </summary>
        /// <returns></returns>
        private int GetAuthenticatedUserId()
        {
            this.Log().Info("===> GetAuthenticatedUserId");

            var authenticatedUserId = _owinContext.Authentication.User.FindFirst(Constants.ClaimTypes.Subject).Value;

            int.TryParse(authenticatedUserId, out var userId);

            return userId;
        }

        /// <summary>
        /// Get parent user id from the claim (revert flow)
        /// </summary>
        /// <returns></returns>
        private int GetParentUserId()
        {
            this.Log().Info("===> GetParentUserId");

            var authenticatedUserImpersonateClaim = GetAuthenticatedUserImpersonateClaim();
            var parentUserId = authenticatedUserImpersonateClaim?.Value;

            int.TryParse(parentUserId, out var userId);

            return userId;
        }

        /// <summary>
        /// Get "impersonate" claim 
        /// </summary>
        /// <returns></returns>
        private Claim GetAuthenticatedUserImpersonateClaim()
        {
            this.Log().Info("===> GetAuthenticatedUserImpersonateClaim");

            var authenticationContext = _owinContext.Authentication.User;
            var authenticatedUserImpersonateClaim =
                authenticationContext?.Claims.FirstOrDefault(claim => claim.Type == "parent_user_id");

            return authenticatedUserImpersonateClaim;
        }
    }
}
