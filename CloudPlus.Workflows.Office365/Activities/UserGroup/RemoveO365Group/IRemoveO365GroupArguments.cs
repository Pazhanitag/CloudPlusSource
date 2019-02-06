using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.RemoveO365Group
{
    public interface IRemoveO365GroupArguments
    {
        string DistributionGroupName { get; set; }
    }
}
