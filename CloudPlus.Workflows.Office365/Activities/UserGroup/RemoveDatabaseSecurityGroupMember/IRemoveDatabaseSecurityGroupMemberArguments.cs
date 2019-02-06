using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseSecurityGroupMember
{
    public interface IRemoveDatabaseSecurityGroupMemberArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> SecurityGroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }
    }
}
