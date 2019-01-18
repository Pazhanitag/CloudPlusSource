namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription
{
    public class SuspendPartnerPlatformSubscriptionLog : ISuspendPartnerPlatformSubscriptionLog
    {
        public string Office365CustomerId { get; set; }
        public string Office365SubscriptionId { get; set; }
    }
}