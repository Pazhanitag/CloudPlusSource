using System;

namespace CloudPlus.Entities.Identity
{
    public class RolePermission : IBaseEntity
    {
        public RolePermission()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
