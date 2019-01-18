namespace CloudPlus.Workflows.Office365.Activities.Customer.PartnerPlatformCustomerSubscription
{
    public interface IPartnerPlatformCustomerSubscriptionLog
    {
        string Office365CustomerId { get; set; }
        string Office365SubscriptionId { get; set; }
        int Quantity { get; set; }
    }
}
