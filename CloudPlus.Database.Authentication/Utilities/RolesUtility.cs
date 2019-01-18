using System.Collections.Generic;
using System.Linq;
using CloudPlus.Entities.Identity;
using CloudPlus.Resources;

namespace CloudPlus.Database.Authentication.Utilities
{
    public class RolesUtility
    {
        private readonly CloudPlusAuthDbContext _context;

        public RolesUtility(CloudPlusAuthDbContext context)
        {
            _context = context;
        }

        public Role MasterAdminRole => _context.Roles.FirstOrDefault(w => w.Name == "MasterAdmin");
        public Role ResellerAdminRole => _context.Roles.FirstOrDefault(w => w.Name == "ResellerAdmin");
        public Role CustomerAdminRole => _context.Roles.FirstOrDefault(w => w.Name == "CustomerAdmin");
        public Role UserRole => _context.Roles.FirstOrDefault(w => w.Name == "User");

        public RolesUtility RemoveRole(Role role)
        {
            if (role == null)
                return this;

            var userRoles = _context.UserRoles.Where(r => r.RoleId == role.Id);

            _context.UserRoles.RemoveRange(userRoles);
           
            _context.SaveChanges();

            var rolePermission = _context.RolePermissions.Where(rp => rp.Role.Id == role.Id);

            _context.RolePermissions.RemoveRange(rolePermission);

            _context.SaveChanges();

            _context.Roles.Remove(role);

            _context.SaveChanges();

            return this;
        }
    }
}
