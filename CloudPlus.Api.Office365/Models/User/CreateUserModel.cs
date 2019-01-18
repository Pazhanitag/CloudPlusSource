using System.ComponentModel.DataAnnotations;
using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.User
{
    public class CreateUserModel : IOffice365Model
    {
        [Required]
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }
        [Required]
        [Office365Name(O365PropertyName = "UserPrincipalName")]
        public string UserPrincipalName { get; set; }
        [Required]
        [Office365Name(O365PropertyName = "DisplayName")]
        public string DisplayName { get; set; }
        [Office365Name(O365PropertyName = "FirstName")]
        public string FirstName { get; set; }
        [Office365Name(O365PropertyName = "LastName")]
        public string LastName { get; set; }
        [Required]
        [Office365Name(O365PropertyName = "UsageLocation")]
        public string UsageLocation { get; set; }
        [Office365Name(O365PropertyName = "City")]
        public string City { get; set; }
        [Office365Name(O365PropertyName = "Country")]
        public string Country { get; set; }
        [Office365Name(O365PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Office365Name(O365PropertyName = "PostalCode")]
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string PostalCode { get; set; }
        [Office365Name(O365PropertyName = "State")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "The field may only contain alpha-numeric characters")]
		public string State { get; set; }
        [Office365Name(O365PropertyName = "StreetAddress")]
        public string StreetAddress { get; set; }
        [Office365Name(O365PropertyName = "Password")]
        public string Password { get; set; }
    }
}
