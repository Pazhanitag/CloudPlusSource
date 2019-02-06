

using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateSecurityGroupMember
{
    public interface ICreateSecurityGroupMemberArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> SecurityGroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }
    }
}
