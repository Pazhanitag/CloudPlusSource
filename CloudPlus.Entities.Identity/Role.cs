using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace CloudPlus.Entities.Identity
{
    public class Role : IdentityRole<int, UserRole>, IBaseEntity
    {
        public Role()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        [MaxLength]
        [JsonIgnore]
        public string AvailableRoles { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
