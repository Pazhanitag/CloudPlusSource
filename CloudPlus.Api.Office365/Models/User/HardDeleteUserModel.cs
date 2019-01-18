using System.ComponentModel.DataAnnotations;
using CloudPlus.Api.Office365.Attributes;

namespace CloudPlus.Api.Office365.Models.User
{
    public class HardDeleteUserModel : IOffice365Model
    {
        [Required]
        [Office365Name(O365PropertyName = "Office365CustomerId")]
        public string Office365CustomerId { get; set; }

        [Required]
        [Office365Name(O365PropertyName = "UserPrincipalName")]
        public string UserPrincipalName { get; set; }
    }
}
