namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__
{
    public class UpdateDatabaseSubscriptionLog : IUpdateDatabaseSubscriptionLog
    {
        public string CloudPlusProductIdentifier { get; set; }
        public string Office365SubscriptionId { get; set; }
        public string Office365OrderId { get; set; }
        public string Office365CustomerId { get; set; }
    }
}