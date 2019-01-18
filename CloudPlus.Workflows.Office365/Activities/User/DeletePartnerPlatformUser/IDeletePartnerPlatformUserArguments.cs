namespace CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser
{
    public interface IDeletePartnerPlatformUserArguments
    {
        int CompanyId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
