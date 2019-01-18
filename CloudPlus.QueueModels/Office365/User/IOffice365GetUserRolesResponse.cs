using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User
{
    public interface IOffice365GetUserRolesResponse
    {
        IEnumerable<string> UserRoles { get; set; }
    }
}
