using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365UserMultiAddViewModel
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public IEnumerable<Office365UserViewModel> Users { get; set; }
        //TAG
        [Required]
        public IEnumerable<string> CloudPlusProductIdentifiers { get; set; }
        [Required]
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<string> SecurityGroupName { get; set; }
        public IEnumerable<string> DistributionGroupName { get; set; }
        public IEnumerable<string> Office365GroupName { get; set; }
    }

    public class Office365UserViewModel
    {
        public string UserPrincipalName { get; set; }
        public string Password { get; set; }
    }
}