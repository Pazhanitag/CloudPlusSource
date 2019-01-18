using System.Collections.Generic;
using CloudPlus.Models.Identity;

namespace CloudPlus.Services.Identity.Permission
{
    public interface IPermissionService
    {
        IEnumerable<PermissionModel> GetUserPermissions(int userId);
    }
}