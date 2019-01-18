using CloudPlus.Database.Authentication;
using CloudPlus.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace CloudPlus.Authentication.Identity.Roles
{
    public class IdentityRoleManager : RoleManager<Role, int>
    {
        public IdentityRoleManager(RoleStore roleStore)
            : base(roleStore)
        {
        }

        public static IdentityRoleManager Create(IdentityFactoryOptions<IdentityRoleManager> options, IOwinContext context)
        {
            var appRoleManager =
                new IdentityRoleManager(new RoleStore(context.Get<CloudPlusAuthDbContext>()));
            return appRoleManager;
        }
    }
}
