using CloudPlus.Models.Office365.Role;
using System.Collections.Generic;

namespace CloudPlus.Services.Database.Office365.Role
{
    public interface IOffice365DbRoleService
    {
        IEnumerable<Office365RoleModel> GetAllRoles();
    }
}
