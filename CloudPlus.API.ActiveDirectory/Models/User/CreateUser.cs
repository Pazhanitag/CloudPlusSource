using System.ComponentModel.DataAnnotations;
using CloudPlus.Api.ActiveDirectory.Attributes;

namespace CloudPlus.Api.ActiveDirectory.Models.User
{
    public class CreateUser : IActiveDirectoryModel
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
	    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		[ActiveDirecotryName(AdPropertyName = "state")]
        public string State { get; set; }
        [ActiveDirecotryName(AdPropertyName = "zip")]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string Zip { get; set; }
        [ActiveDirecotryName(AdPropertyName = "country")]
        public string CountryCode { get; set; }
        [ActiveDirecotryName(AdPropertyName = "displayName")]
        [Required]
        public string DisplayName { get; set; }
        [ActiveDirecotryName(AdPropertyName = "upn")]
        [Required]
        [EmailAddress]
        public string Upn { get; set; }
        [ActiveDirecotryName(AdPropertyName = "password")]
        [Required]
        public string Password { get; set; }
        [ActiveDirecotryName(AdPropertyName = "customerAccountId")]
        [Required]
        public string CompanyOu { get; set; }
        [ActiveDirecotryName(AdPropertyName = "customerPrimaryDomain")]
        public string CompanyDomain { get; set; }
        [ActiveDirecotryName(AdPropertyName = "title")]
        public string JobTitle { get; set; }
        [ActiveDirecotryName(AdPropertyName = "company")]
        public string Company { get; set; }
        [ActiveDirecotryName(AdPropertyName = "emailAddress")]
        public string EmailAddress { get; set; }
    }
}