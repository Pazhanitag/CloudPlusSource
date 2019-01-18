using CloudPlus.Entities.Identity;
using CloudPlus.Database.Authentication;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Authentication.Identity.Users
{
    public class UserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public UserStore(CloudPlusAuthDbContext context)
            : base(context)
        {

        }
    }
}
