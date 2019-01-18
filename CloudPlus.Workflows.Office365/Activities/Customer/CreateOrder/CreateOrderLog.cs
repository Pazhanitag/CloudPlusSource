namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder
{
    public class CreateOrderLog : ICreateOrderLog
    {
        public string Office365CustomerId { get; set; }
        public string Office365SubscriptionId { get; set; }
    }
}