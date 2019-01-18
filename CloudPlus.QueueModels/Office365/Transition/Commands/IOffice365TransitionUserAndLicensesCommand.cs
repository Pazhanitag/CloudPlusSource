using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.Transition.Commands
{
    public interface IOffice365TransitionUserAndLicensesCommand
    {
        bool Admin { get; set; }
        bool RemoveLicenses { get; set; }
        bool CloudPlusUserExist { get; set; }
        bool IsNewLicenses { get; set; }

        int CompanyId { get; set; }
        bool KeepLicences { get; set; }
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
        string Office365UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayName { get; set; }
        IEnumerable<string> UserRoles { get; set; }
        List<int> Roles { get; set; }
        string Password { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
