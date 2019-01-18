using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Models.Office365.Role;

namespace CloudPlus.Api.Mappers
{
    public static class Office365RoleMapper
    {
        public static Office365RoleViewModel ToOffice365RoleViewModel(this Office365RoleModel role)
        {
            if (role == null)
                return null;

            return new Office365RoleViewModel
            {
                Name = role.Name,
                DisplayName = role.DisplayName
            };
        }
    }
}