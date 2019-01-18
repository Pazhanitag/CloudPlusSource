namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder
{
    public interface ICreateOrderArguments
    {
        string CloudPlusProductIdentifier { get; set; }
        string Office365CustomerId { get; set; }
        int Quantity { get; set; }
    }
}