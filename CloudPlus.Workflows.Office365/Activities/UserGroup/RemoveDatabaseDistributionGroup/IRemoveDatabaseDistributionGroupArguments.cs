using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseDistributionGroup
{
    public interface IRemoveDatabaseDistributionGroupArguments 
    {
        string Office365CustomerId { get; set; }
        string DistributionGroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }

    }
}
