using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseO365Group
{
    public interface IRemoveDatabaseO365GroupArguments
    {
        string Office365CustomerId { get; set; }
        string Office365GroupName { get; set; }
        string CustomerO365Domain { get; set; }
        string UserSMTPAddress { get; set; }
    }
}
