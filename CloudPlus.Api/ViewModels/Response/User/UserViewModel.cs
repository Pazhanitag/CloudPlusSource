using System.Collections.Generic;
using System.Linq;
using CloudPlus.Api.ViewModels.Response.Roles;
using CloudPlus.Enums.User;

namespace CloudPlus.Api.ViewModels.Response.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string AlternativeEmail { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public UserStatus UserStatus { get; set; }
        public string UserStatusDisplay => UserStatus.ToString();
        public string Role => Roles.FirstOrDefault()?.FriendlyName;
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}