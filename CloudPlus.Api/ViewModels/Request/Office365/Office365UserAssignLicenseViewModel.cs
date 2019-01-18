using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365UserAssignLicenseViewModel
    {
        [Required]
        public int CompanyId { get; set; }
        //[Required]
        public string CloudPlusProductIdentifier { get; set; }
        [Required]
        public string UserPrincipalName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UsageLocation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string StreetAddress { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}