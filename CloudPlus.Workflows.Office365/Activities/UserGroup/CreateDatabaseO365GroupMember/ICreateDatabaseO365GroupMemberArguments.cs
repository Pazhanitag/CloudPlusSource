using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseO365GroupMember
{
    public interface ICreateDatabaseO365GroupMemberArguments
    {
        string Office365CustomerId { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> Office365GroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }
    }
}
