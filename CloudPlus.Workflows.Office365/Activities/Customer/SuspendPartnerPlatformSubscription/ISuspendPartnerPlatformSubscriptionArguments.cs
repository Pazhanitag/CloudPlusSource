namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription
{
    public interface ISuspendPartnerPlatformSubscriptionArguments
    {
        string Office365CustomerId { get; set; }
        string Office365SubscriptionId { get; set; }
    }
}