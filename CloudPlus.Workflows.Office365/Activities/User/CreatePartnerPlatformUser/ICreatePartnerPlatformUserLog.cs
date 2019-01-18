namespace CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser
{
    public interface ICreatePartnerPlatformUserLog
    {
        string Office365CustomerId { get; set; }
        string UserPrincipalName { get; set; }
    }
}
