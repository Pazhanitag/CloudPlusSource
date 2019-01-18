using System;
using System.Collections.Generic;
using System.Linq;
using CloudPlus.Infrastructure.Cache;
using CloudPlus.Models.Identity;
using CloudPlus.Resources;
using CloudPlus.Services.Identity.Permission;

namespace CloudPlus.Api.Security
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly ICacheStore _cacheStore;
        private readonly IConfigurationManager _configurationManager;
        private readonly IPermissionService _permissionService;

        public AuthorizationManager(
            IConfigurationManager configurationManager,
            ICacheStore cacheStore,
            IPermissionService permissionService)
        {
            _configurationManager = configurationManager;
            _cacheStore = cacheStore;
            _permissionService = permissionService;
        }

        public bool HasAccessByPermission(string []permissions, int userId)
        {
            if (!permissions.Any())
                throw new ArgumentNullException(nameof(permissions));

            try
            {
                var dbPermissions = GetPermissions(userId);

                if (dbPermissions == null)
                {
                    _cacheStore.Remove(userId.ToString());

                    return false;
                }

                var hasAccess = dbPermissions.Any(permission => permissions.Contains(permission.Name));

                return hasAccess;
            }
            catch
            {
                _cacheStore.Remove(userId.ToString());

                return false;
            }
        }

        private List<PermissionModel> GetPermissions(int userId)
        {
            int.TryParse(_configurationManager.GetByKey("Authorize.CacheExpireInMinutes"), out var expirationMinutes);

			var response = _permissionService.GetUserPermissions(userId);

	        return response.ToList();
		}
    }
}