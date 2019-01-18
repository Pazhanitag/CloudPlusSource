using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IBaseEntity
    {
        public UserClaim()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
