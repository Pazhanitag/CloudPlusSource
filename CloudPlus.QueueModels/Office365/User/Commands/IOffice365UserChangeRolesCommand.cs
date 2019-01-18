using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserChangeRolesCommand
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }
}
