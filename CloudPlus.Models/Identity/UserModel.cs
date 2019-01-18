using System.Collections.Generic;
using CloudPlus.Enums.Provisions;
using CloudPlus.Enums.User;

namespace CloudPlus.Models.Identity
{
    public class UserModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AlternativeEmail { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }

        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public UserStatus UserStatus { get; set; }
        public string ProfilePicture { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        //TODO This should be refactored. This status is Office service status
        public bool IsProvisioned { get; set; }
        //TODO This should be refactored together with above code, this is concerned with office365 licenses
        public string AssignedLicense { get; set; } = "No package assigned";
    }
}
