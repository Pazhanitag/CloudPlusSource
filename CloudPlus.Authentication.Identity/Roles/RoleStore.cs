using CloudPlus.Database.Authentication;
using CloudPlus.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Authentication.Identity.Roles
{
    public class RoleStore : RoleStore<Role, int, UserRole>
    {
        public RoleStore(CloudPlusAuthDbContext context) : base(context)
        {
        }
    }
}
