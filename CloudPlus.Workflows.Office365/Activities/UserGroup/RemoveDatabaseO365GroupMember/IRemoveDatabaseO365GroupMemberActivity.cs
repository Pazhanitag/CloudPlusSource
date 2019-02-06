using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveDatabaseO365GroupMember
{
    public interface IRemoveDatabaseO365GroupMemberActivity : ExecuteActivity<IRemoveDatabaseO365GroupMemberArguments>
    {
    }
}
