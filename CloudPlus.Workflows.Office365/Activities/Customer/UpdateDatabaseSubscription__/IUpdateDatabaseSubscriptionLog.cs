namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscription__
{
    public interface IUpdateDatabaseSubscriptionLog
    {
        string CloudPlusProductIdentifier { get; set; }
        string Office365SubscriptionId { get; set; }
        string Office365OrderId { get; set; }
        string Office365CustomerId { get; set; }
    }
}