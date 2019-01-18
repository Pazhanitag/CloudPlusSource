namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription
{
    public interface ISuspendPartnerPlatformSubscriptionLog
    {
        string Office365CustomerId { get; set; }
        string Office365SubscriptionId { get; set; }
    }
}