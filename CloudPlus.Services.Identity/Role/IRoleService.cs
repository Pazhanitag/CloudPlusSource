using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Identity;

namespace CloudPlus.Services.Identity.Role
{
    public interface IRoleService
    {
        IEnumerable<RoleModel> GetAllRoles();
        Task<IEnumerable<RoleModel>> GetAvailableRolesAsync(int roleId);
    }
}
