using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserCreateCommand
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
        string UsageLocation { get; set; }
        string Password { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }
}