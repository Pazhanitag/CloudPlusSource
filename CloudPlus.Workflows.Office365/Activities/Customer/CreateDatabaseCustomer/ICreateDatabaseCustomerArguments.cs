namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer
{
    public interface ICreateDatabaseCustomerArguments
    {
        int CompanyId { get; set; }
        string Office365CustomerId { get; set; }
    }
}
