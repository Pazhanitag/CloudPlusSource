using System;
using CloudPlus.Enums.User;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Entities.Identity
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IBaseEntity
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsApproved = true;
            IsLockedOut = false;
        }

        public override string UserName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsLockedOut { get; set; }
        public string Discriminator { get; set; }
        public bool IsDeleted { get; set; }
        public UserStatus UserStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int CompanyId { get; set; }
        public string AlternativeEmail { get; set; }
        public string ProfilePicture { get; set; }
        public string CompanyName { get; set; }

    }
}
