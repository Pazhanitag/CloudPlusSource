using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.UserGroup.CreateDistriputionGroup
{
    public interface ICreateDistributionGroupArguments
    {
        string DistributionGroupName { get; set; }
        string CurrentUserPrincipalName { get; set; }
        string AssignToUserPrincipalName { get; set; }
        string MemberJoinPolicy { get; set; }
    }
}