namespace CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser
{
    public class DeletePartnerPlatformUserLog : IDeletePartnerPlatformUserLog
    {
        public string Office365UserId { get; set; }
        public string Office365CustomerId { get; set; }
    }
}
