using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<int>, IBaseEntity
    {
        public UserLogin()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
