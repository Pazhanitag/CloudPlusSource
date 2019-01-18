using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365UserMultiEditViewModel
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public IEnumerable<Office365UserViewModel> Users { get; set; }
        public string CloudPlusProductIdentifier { get; set; }
        [Required]
        public IEnumerable<string> UserRoles { get; set; }
    }

    public class Office365UserViewModel
    {
        public string UserPrincipalName { get; set; }
        public string Password { get; set; }
    }
}