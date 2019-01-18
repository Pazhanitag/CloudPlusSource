namespace CloudPlus.Workflows.Office365.Activities.User.RestorePartnerPlatformUser
{
    public interface IRestorePartnerPlatformUserArguments
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
