using System.ComponentModel.DataAnnotations;
using CloudPlus.Api.ActiveDirectory.Attributes;

namespace CloudPlus.Api.ActiveDirectory.Models.User
{
    public class UpdateUser : IActiveDirectoryModel
    {
        [ActiveDirecotryName(AdPropertyName = "givenName")]
        public string FirstName { get; set; }
        [ActiveDirecotryName(AdPropertyName = "surName")]
        public string LastName { get; set; }
        [ActiveDirecotryName(AdPropertyName = "phone")]
        public string PhoneNumber { get; set; }
        [ActiveDirecotryName(AdPropertyName = "address")]
        public string Address { get; set; }
        [ActiveDirecotryName(AdPropertyName = "city")]
        public string City { get; set; }
        [ActiveDirecotryName(AdPropertyName = "state")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string State { get; set; }
        [ActiveDirecotryName(AdPropertyName = "zip")]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string Zip { get; set; }
        [ActiveDirecotryName(AdPropertyName = "country")]
        public string CountryCode { get; set; }
        [ActiveDirecotryName(AdPropertyName = "displayName")]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        [ActiveDirecotryName(AdPropertyName = "upn")]
        public string Upn { get; set; }
        [ActiveDirecotryName(AdPropertyName = "title")]
        public string JobTitle { get; set; }
        [ActiveDirecotryName(AdPropertyName = "company")]
        public string Company { get; set; }
        [ActiveDirecotryName(AdPropertyName = "emailAddress")]
        public string EmailAddress { get; set; }
    }
}