using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserMultiEditCommand
    {
        int CompanyId { get; set; }
        IEnumerable<string> UserPrincipalNames { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }
}
