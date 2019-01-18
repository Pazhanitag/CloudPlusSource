namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity
{
    public class UpdateDatabaseSubscriptionQuantityLog : IUpdateDatabaseSubscriptionQuantityLog
    {
        public string Office365SubscriptionId { get; set; }
        public int QuantityChange { get; set; }
    }
}