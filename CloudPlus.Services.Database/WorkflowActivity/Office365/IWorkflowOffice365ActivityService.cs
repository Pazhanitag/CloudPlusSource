using CloudPlus.Models.Enums;

namespace CloudPlus.Services.Database.WorkflowActivity.Office365
{
    public interface IWorkflowOffice365ActivityService
    {
        bool IsOffice365ProvisioningInProgress(int companyId);
        bool IsOffice365DomainVerificationInProgress(string domain);
        bool IsOffice365AddingAdditionalDomainInProgress(string domain);
        bool IsOffice365UserLicenceAssignmentInProgress(string userPrincipalName);
        bool IsOffice365UserLicenceChangingInProgress(string userPrincipalName);
        bool IsOffice365UserLicenceRemovalInProgress(string userPrincipalName);
        bool IsOffice365UserLicenceRestoreInProgress(string userPrincipalName);
        bool IsOffice365ManageSubscriptionInProgress(string userPrincipalName);

        bool IsOffice365UserLicenceAssignmentForLicenseInProgress(string userPrincipalName, string License);
        bool IsOffice365UserLicenceChangingForLicenseInProgress(string userPrincipalName, string License);
        bool IsOffice365UserLicenceRemovalForLicenseInProgress(string userPrincipalName, string License);
        bool IsOffice365UserLicenceRestoreForLicenseInProgress(string userPrincipalName, string License);
        bool IsOffice365ManageSubscriptionForLicenseInProgress(string userPrincipalName, string License);

        bool IsOffice365UserRolesChangingInProgress(string userPrincipalName);
        bool IsOffice365TransitionInProgress(int companyId);
        bool IsOffice365TransitionStarted(int companyId);
        bool IsOffice365TransitionDeletePartnerPlatformUserInProgress(string userPrincipalName);
        bool IsOffice365TransitionUserAndLicensesInProgress(string userPrincipalName);
        WorkflowActivityStatus Office365TransitionDeletePartnerPlatformUserStatus(string userPrincipalName);
        WorkflowActivityStatus Office365TransitionUserAndLicensesStatus(string userPrincipalName);
    }
}
