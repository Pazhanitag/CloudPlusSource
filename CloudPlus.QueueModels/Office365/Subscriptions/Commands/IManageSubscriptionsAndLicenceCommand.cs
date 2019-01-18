using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.Subscriptions.Commands
{
    public interface IManageSubscriptionsAndLicencesCommand
    {
        int CompanyId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
        IEnumerable<Office365LicenceUser> Users { get; set; }
        IEnumerable<string> UserRoles { get; set; }
        ManageSubsctiptionAndLicenceCommandType MessageType { get; set; }
    }

    public class Office365LicenceUser
    {
        public string UserPrincipalName { get; set; }
        public string Password { get; set; }
    }
    public enum ManageSubsctiptionAndLicenceCommandType
    {
        AssignNewLicence = 0,
        MultiEditUser = 1,
        ChangeLicence = 3,
        RemoveLicence = 4,
        RestoreUser = 5,
        HardDeleteUser = 6
    }
}