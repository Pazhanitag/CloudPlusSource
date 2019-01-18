using System.Collections.Generic;
using System.Linq;
using CloudPlus.Database.Authentication;
using CloudPlus.Models.Identity;

namespace CloudPlus.Services.Identity.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly CloudPlusAuthDbContext _dbContext;

        public PermissionService(CloudPlusAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<PermissionModel> GetUserPermissions(int userId)
        {
            return from ur in _dbContext.UserRoles
                   join rp in _dbContext.RolePermissions on ur.RoleId equals rp.Role.Id
                   where ur.UserId.Equals(userId)
                   select new PermissionModel
                   {
                       Id = rp.Permission.Id,
                       Name = rp.Permission.Name
                   };
        }
    }
}
