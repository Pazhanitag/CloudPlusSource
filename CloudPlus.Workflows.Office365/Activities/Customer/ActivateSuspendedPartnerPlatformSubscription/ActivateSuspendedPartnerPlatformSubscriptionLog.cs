namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedPartnerPlatformSubscription
{
    public class ActivateSuspendedPartnerPlatformSubscriptionLog : IActivateSuspendedPartnerPlatformSubscriptionLog
    {
        public string Office365SubscriptionId { get; set; }
        public string Office365CustomerId { get; set; }
    }
}