namespace CloudPlus.Models.Enums
{
    public enum WorkflowActivityStep
    {
        CreateActiveDirectoryUser,
        CreateLocalUser,
        CreateActiveDirectoryCompany,
        CreateDatabaseCompany,
        UpdateActiveDirectoryUser,
        UpdateLocalUser,
		DeleteActiveDirectoryUser,
		DeleteLocalUser,
        UpdateDatabaseCompany,
        SendRegistrationEmail,

        #region Office 365
        Office365CreatePartnerPlatformCustomer,
        Office365CreateDatabaseCustomer,
        Office365PartnerPlatformCustomerSubscription,
        Office365DatabaseCustomerSubscription,
        Office365UpdateDatabaseCustomerSubscription,
        Office365DecreasePartnerPlatformCustomerSubscription,
        Office365DecreaseDatabaseCustomerSubscription,

        Office365AddCustomerDomainPartnerPlatform,
        Office365AddCustomerDomainDatabase,
        Office365GetCustomerTxtRecords,
        Office365SendCustomerTxtRecords,
        Office365VerifyCustomerDomain,
        Office365VerifyCustomerDomainDatabaseStatus,
        Office365FederateCustomerDomain,
        Office365FederateCustomerDomainDatabaseStatus,

        Office365CreatePartnerPlatformUser,
        Office365CreateDatabaseUser,
        Office365CreateTempPartnerPlatformAdminUser,
        Office365DeleteDatabaseUser,
        Office365DeletePartnerPlatformUser,
        Office365HardDeletePartnerPortalUser,
        Office365HardDeleteDatabaseUser,
        Office365SoftDeleteDatabaseUser,
        Office365RestorePartnerPlatformUser,
        Office365ActivateSoftDeletedDatabaseUser,
        Office365GetUserRoles,
        Office365AssignUserRoles,
        Office365RemoveUserRoles,
        Office365AssignLicenseToPartnerPlatformUser,
        Office365AssignLicenseToDatabaseUser,
        Office365RemoveLicensePartnerPortalUser,
        Office365RemoveAllLicensePartnerPortalUser,
        Office365RemoveLicenseDatabaseUser,
        Office365SendUserSetupEmail,

        Office365TransitionDispatchCreatingUsers,
        Office365DatabaseProvisionedStatusProvisioned,
        #endregion

        Error
    }
}
