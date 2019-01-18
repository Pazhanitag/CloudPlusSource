namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedPartnerPlatformSubscription
{
    public interface IActivateSuspendedPartnerPlatformSubscriptionLog
    {
        string Office365SubscriptionId { get; set; }
        string Office365CustomerId { get; set; }
    }
}