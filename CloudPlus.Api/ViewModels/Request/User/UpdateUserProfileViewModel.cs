using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CloudPlus.Enums.User;

namespace CloudPlus.Api.ViewModels.Request.User
{
    public class UpdateUserProfileViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string DisplayName { get; set; }
        [EmailAddress]
        public string AlternativeEmail { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string AvatarBase64 { get; set; }
        public string ProfilePicture { get; set; }
        public int CompanyId { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}