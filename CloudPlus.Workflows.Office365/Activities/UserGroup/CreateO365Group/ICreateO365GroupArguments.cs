using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateO365Group
{
    public interface ICreateO365GroupArguments
    {
        string DistributionGroupName { get; set; }
        string CurrentUserPrincipalName { get; set; }
        string AssignToUserPrincipalName { get; set; }
        string MemberJoinPolicy { get; set; }
    }
}
