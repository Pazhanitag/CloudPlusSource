using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User
{
    public class Office365GetUserRolesResponse : IOffice365GetUserRolesResponse
    {
        public IEnumerable<string> UserRoles { get; set; }
    }
}
