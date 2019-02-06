using MassTransit.Courier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDatabaseDistributionGroupMember
{
    public interface ICreateDatabaseDistributionGroupMemberActivity : ExecuteActivity<ICreateDatabaseDistributionGroupMemberArguments>
    {
    }
}
