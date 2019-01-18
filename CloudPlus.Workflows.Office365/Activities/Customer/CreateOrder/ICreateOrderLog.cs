namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder
{
    public interface ICreateOrderLog
    {
        string Office365CustomerId { get; set; }
        string Office365SubscriptionId { get; set; }
    }
}