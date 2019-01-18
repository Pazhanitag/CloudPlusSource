namespace CloudPlus.Workflows.Office365.Activities.User.RemoveLicensePartnerPortalUser
{
    public interface IRemoveLicensePartnerPortalUserArguments
    {
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
