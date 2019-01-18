namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseSubscriptionQuantity
{
    public interface IUpdateDatabaseSubscriptionQuantityArguments
    {
        string Office365SubscriptionId { get; set; }
        int QuantityChange { get; set; }
    }
}