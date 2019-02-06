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
        Office365TransitionDeletePartnerPlatformUser,
        Office365CreateSecurityGroup,
        Office365CreateDistributionGroup,
        Office365CreateO365Group,
        Office365AddSecurityGroupMember,
        Office365AddDistributionGroupMember,
        Office365AddO365GroupMember,
        Office365RemoveSecurityGroup,
        Office365RemoveDistributionGroup,
        Office365RemoveO365Group,
        Office365RemoveSecurityGroupMember,
        Office365RemoveDistributionGroupMember,
        Office365RemoveO365GroupMember
        #endregion
    }
}