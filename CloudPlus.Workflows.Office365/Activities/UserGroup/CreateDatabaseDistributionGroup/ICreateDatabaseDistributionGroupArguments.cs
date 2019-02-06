using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroup
{
    public interface ICreateDatabaseDistributionGroupArguments
    {
        string Office365CustomerId { get; set; }
        string DistributionGroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }

        
    }
}
