namespace CloudPlus.Workflows.Office365.Activities.User.AssignLicenseToPartnerPlatformUser
{
    public interface IAssignLicenseToPartnerPlatformUserArguments
    {
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
