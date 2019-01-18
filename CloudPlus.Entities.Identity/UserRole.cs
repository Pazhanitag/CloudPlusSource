using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Entities.Identity
{
    public class UserRole : IdentityUserRole<int>, IBaseEntity
    {
        public UserRole()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
