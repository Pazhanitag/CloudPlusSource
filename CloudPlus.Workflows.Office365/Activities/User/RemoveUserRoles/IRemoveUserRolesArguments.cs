using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles
{
    public interface IRemoveUserRolesArguments
    {
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
        IEnumerable<string> CurrentAssignedRoles { get; set; }
    }
}
