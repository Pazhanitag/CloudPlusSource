namespace CloudPlus.Enums.Notification
{
    public enum EmailTemplateType
    {
        None,
        WelcomeUserPasswordViaEmail,
        WelcomeUserDontSendPassword,
        WelcomeUserSendPlainPasswordViaEmail,
        WelcomeCompanyCustomer,
        WelcomeCompanyCustomerSendPlainPasswordViaEmail,
        WelcomeCompanyCustomerPasswordViaEmail,
        WelcomeCompanyReseller,
        WelcomeCompanyResellerSendPlainPasswordViaEmail,
        WelcomeCompanyResellerPasswordViaEmail,
        ForgotPassword,
        PasswordChanged,
        PasswordChangedSendPlainPasswordViaEmail,
        ChangePassword,

        Office365CustomerServiceEnabled,
        Office365PrimaryDomainVerifiedSetUp,
        Office365AdditionalDomainVerified,
        Office365UserSetUp,
        Office365TransitionReport,

        CustomSecureControlPanelActivation
    }
}
