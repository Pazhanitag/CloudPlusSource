using CloudPlus.Api.ViewModels.Response.Permissions;
using CloudPlus.Models.Identity;

namespace CloudPlus.Api.Mappers
{
    public static class PermissionMapper
    {
        public static PermissionViewModel ToPermissionViewModel(this PermissionModel permission)
        {
            if (permission == null)
                return null;

            return new PermissionViewModel
            {
                Id = permission.Id,
                Name = permission.Name
            };
        }
    }
}