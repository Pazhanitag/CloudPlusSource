using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.User.Commands
{
    public interface IOffice365UserAssignLicenseCommand
    {
        int CompanyId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        string UserPrincipalName { get; set; }
        string DisplayName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string UsageLocation { get; set; }
        string City { get; set; }
        string Country { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        string State { get; set; }
        string StreetAddress { get; set; }
        string Password { get; set; }
        IEnumerable<string> UserRoles { get; set; }
    }
}
