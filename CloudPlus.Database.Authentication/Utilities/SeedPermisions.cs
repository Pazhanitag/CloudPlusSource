using CloudPlus.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Database.Authentication.Utilities
{
    public class SeedPermisions
    {
        private readonly CloudPlusAuthDbContext _context;
        private readonly RolesUtility _rolesUtility;
        private readonly PermissionsUtility _permissionsUtility;

        public SeedPermisions(CloudPlusAuthDbContext context)
        {
            _context = context;
            _rolesUtility = new RolesUtility(context);
            _permissionsUtility = new PermissionsUtility(context);
        }

        public SeedPermisions UpdatePermisionsExternalSignup()
        {
            _context.Permissions.AddOrUpdate(x => x.Name,
              new Permission
              {
                  Name = "EditMyCompany"
              },
              new Permission
              {
                  Name = "ViewExternalSignupLink"
              });
            _context.SaveChanges();
            _context.RolePermissions.AddOrUpdate(
              new RolePermission
              {
                  Role = _rolesUtility.MasterAdminRole,
                  Permission = _permissionsUtility.EditMyCompany
              },
              new RolePermission
              {
                  Role = _rolesUtility.ResellerAdminRole,
                  Permission = _permissionsUtility.ViewExternalSignupLink
              },
              new RolePermission
              {
                  Role = _rolesUtility.MasterAdminRole,
                  Permission = _permissionsUtility.ViewExternalSignupLink
              },
              new RolePermission
              {
                  Role = _rolesUtility.ResellerAdminRole,
                  Permission = _permissionsUtility.EditMyCompany
              },
              new RolePermission
              {
                  Role = _rolesUtility.CustomerAdminRole,
                  Permission = _permissionsUtility.EditMyCompany
              });
            _context.SaveChanges();
            return this;
        }

    }
}
