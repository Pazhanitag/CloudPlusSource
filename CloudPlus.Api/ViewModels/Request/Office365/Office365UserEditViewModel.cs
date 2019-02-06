using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365UserEditViewModel
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public Office365UserViewModel User { get; set; }
        [Required]
        public IEnumerable<string> CloudPlusProductIdentifiers { get; set; }
        [Required]
        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<string> SecurityGroupName { get; set; }
        public IEnumerable<string> DistributionGroupName { get; set; }
        public IEnumerable<string> Office365GroupName { get; set; }
    }
}