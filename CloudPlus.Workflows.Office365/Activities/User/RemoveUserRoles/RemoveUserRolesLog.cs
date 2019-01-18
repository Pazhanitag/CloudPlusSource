using System.Collections.Generic;

namespace CloudPlus.Workflows.Office365.Activities.User.RemoveUserRoles
{
    public class RemoveUserRolesLog : IRemoveUserRolesLog
    {
        public string Office365CustomerId { get; set; }
        public string UserPrincipalName { get; set; }
        public IEnumerable<string> CurrentAssignedRoles { get; set; }
    }
}
