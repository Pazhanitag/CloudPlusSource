using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.User.AssignUserRoles
{
    public interface IAssignUserRolesArguments
    {
        IEnumerable<string> UserRoles { get; set; }
        string UserPrincipalName { get; set; }
        string Office365CustomerId { get; set; }
    }
}