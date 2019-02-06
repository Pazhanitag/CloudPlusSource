using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroupMember
{
    public interface ICreateDatabaseDistributionGroupMemberArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> DistributionGroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }
    }
}
