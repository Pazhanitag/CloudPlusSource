namespace CloudPlus.Workflows.Office365.Activities.Customer.UpdateDatabaseCustomerSubscription
{
    public interface IUpdateDatabaseCustomerSubscriptionArguments
    {
        string CloudPlusProductIdentifier { get; set; }
        string Office365SubscriptionId { get; set; }
        string Office365OrderId { get; set; }
        //string Office365FriendlyName { get; set; }
        //int Quantity { get; set; }
        string Office365CustomerId { get; set; }
    }
}
