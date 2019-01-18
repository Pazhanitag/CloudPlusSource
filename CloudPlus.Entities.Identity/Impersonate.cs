using System;

namespace CloudPlus.Entities.Identity
{
    public class Impersonate: IBaseEntity
    {
        public Impersonate()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public User ParentUser { get; set; }
        public User ImpersonateUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
