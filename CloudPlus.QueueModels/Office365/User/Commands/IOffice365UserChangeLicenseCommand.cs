using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserChangeLicenseCommand
    {
        int CompanyId { get; set; }
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
        string RemoveCloudPlusProductIdentifier { get; set; }
        string AssignCloudPlusProductIdentifier { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }
}
