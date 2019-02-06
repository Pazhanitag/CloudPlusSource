using System.Collections.Generic;

namespace CloudPlus.QueueModels.Office365.Subscriptions.Commands
{
    public interface IManageSubscriptionsAndLicencesCommand
    {
        int CompanyId { get; set; }
        //TAG
        IEnumerable<string> CloudPlusProductIdentifiers { get; set; }
        IEnumerable<IOffice365LicenceUser> Users { get; set; }
        IEnumerable<string> UserRoles { get; set; }
        IEnumerable<string> SecurityGroupName { get; set; }
        IEnumerable<string> DistributionGroupName { get; set; }
        IEnumerable<string> Office365GroupName { get; set; }
        ManageSubsctiptionAndLicenceCommandType MessageType { get; set; }
    }

    public interface IOffice365LicenceUser
    {
        string UserPrincipalName { get; set; }
        string Password { get; set; }
    }
    public enum ManageSubsctiptionAndLicenceCommandType
    {
        AssignNewLicence = 0,
        MultiAddUser = 1,
		EditUser=2,
        ChangeLicence = 3,
        RemoveLicence = 4,
        RestoreUser = 5,
        HardDeleteUser = 6
    }
}