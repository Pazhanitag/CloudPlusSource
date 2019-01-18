using CloudPlus.Database;
using CloudPlus.Models.Office365.Role;
using System.Collections.Generic;
using System.Linq;

namespace CloudPlus.Services.Database.Office365.Role
{
    public class Office365DbRoleService : IOffice365DbRoleService
    {
        private readonly CldpDbContext _dbContext;
        public Office365DbRoleService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Office365RoleModel> GetAllRoles()
        {
            var roles = _dbContext.Office365Roles;
            return roles.OrderBy(r => r.Ord).Select(role => new Office365RoleModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                Office365Id = role.Office365Id,
                DisplayName = role.DisplayName
            });
        }
    }
}
