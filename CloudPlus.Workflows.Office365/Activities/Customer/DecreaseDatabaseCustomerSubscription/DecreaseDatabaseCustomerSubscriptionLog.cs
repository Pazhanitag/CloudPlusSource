namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription
{
    public class DecreaseDatabaseCustomerSubscriptionLog : IDecreaseDatabaseCustomerSubscriptionLog
    {
        public string Office365CustomerId { get; set; }
        public string CloudPlusProductIdentifier { get; set; }
        public string Office365SubscriptionId { get; set; }
        public string Office365OrderId { get; set; }
        public string Office365FriendlyName { get; set; }
    }
}
