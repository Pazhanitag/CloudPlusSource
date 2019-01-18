using CloudPlus.Api.ViewModels.Response.Roles;
using CloudPlus.Models.Identity;

namespace CloudPlus.Api.Mappers
{
    public static class RoleMapper
    {
        public static RoleViewModel ToRoleViewModel(this RoleModel role)
        {
            if (role == null)
                return null;

            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                FriendlyName = role.FriendlyName,
                Description = role.Description
            };
        }
    }
}