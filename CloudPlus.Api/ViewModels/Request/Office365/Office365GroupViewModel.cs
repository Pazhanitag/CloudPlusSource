using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.ViewModels.Request.Office365
{
    public class Office365SecurityGroupViewModel
    {
        public string SecurityGroupName { get; set; }
        public int CompanyId { get; set; }
    }

    public class Office365DistributionGroupViewModel
    {
        public string DistributionGroupName { get; set; }
        public int CompanyId { get; set; }
    }

    public class Office365Office365GroupViewModel
    {
        public string Office365GroupName { get; set; }
        public int CompanyId { get; set; }
    }
}