namespace CloudPlus.Workflows.Office365.Activities.User.DeletePartnerPlatformUser
{
    public interface IDeletePartnerPlatformUserLog
    {
        string Office365UserId { get; set; }
        string Office365CustomerId { get; set; }
    }
}
