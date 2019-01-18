using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database.Authentication;
using CloudPlus.Models.Identity;
using CloudPlus.Resources;

namespace CloudPlus.Services.Identity.Role
{
    public class RoleService : IRoleService
    {
        private readonly CloudPlusAuthDbContext _dbContext;
        private readonly IObjectSerializer _objectSerializer;
        public RoleService(CloudPlusAuthDbContext dbContext, IObjectSerializer objectSerializer)
        {
            _dbContext = dbContext;
            _objectSerializer = objectSerializer;
        }

        public IEnumerable<RoleModel> GetAllRoles()
        {
            return _dbContext.Roles.Select(r => new RoleModel
            {
                Id = r.Id,
                Name = r.Name,
                FriendlyName = r.FriendlyName
            });
        }

        public async Task<IEnumerable<RoleModel>> GetAvailableRolesAsync(int roleId)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var availableRoles = _objectSerializer.Deserialize<IEnumerable<Entities.Identity.Role>>(role.AvailableRoles);

            return availableRoles.Select(r => new RoleModel
            {
                Id = r.Id,
                Name = r.Name,
                FriendlyName = r.FriendlyName,
                Description = r.Description
            });
        }
    }
}
