namespace CloudPlus.Models.Enums
{
    public enum WorkflowActivityType
    {
        CreateUser,
        UpdateUser,
		DeleteUser,
        CreateCompany,
        UpdateCompany,

        #region Office 365
        CreateOffice365Customer,
        Office365CreateUser,
        Office365HardDeleteUser,
        ResendOffice365DomainTxtRecords,
        VerifyAndFederateOffice365Domain,
        FederateOffice365Domain,
        Office365AddAdditionalDomain,
        Office365ManageSubscription,
        Office365UserAssignLicense,
        Office365UserRemoveLicense,
        Office365UserChangeLicense,
        Office365UserRestore,
        Office365UserChangeRoles,
        Office365Transition,
        Office365TransitionUserAndLicenses,
        Office365TransitionDeletePartnerPlatformUser
        #endregion
    }
}