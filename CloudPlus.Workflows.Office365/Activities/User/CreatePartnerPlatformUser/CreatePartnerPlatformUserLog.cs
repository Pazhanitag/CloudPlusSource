namespace CloudPlus.Workflows.Office365.Activities.User.CreatePartnerPlatformUser
{
    public class CreatePartnerPlatformUserLog : ICreatePartnerPlatformUserLog
    {
        public string Office365CustomerId { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
