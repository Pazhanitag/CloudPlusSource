namespace CloudPlus.Workflows.Office365.Activities.User.HardDeletePartnerPlatformUser
{
    public interface IHardDeletePartnerPlatformUserArguments
    {
        string UserPrincipalName { get; set; }
        string Office365CustomerId { get; set; }
        bool SwallowException { get; set; }
    }
}
