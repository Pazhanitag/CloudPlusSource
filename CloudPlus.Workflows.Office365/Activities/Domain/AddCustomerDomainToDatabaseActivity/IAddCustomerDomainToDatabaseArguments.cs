namespace CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainToDatabaseActivity
{
    public interface IAddCustomerDomainToDatabaseArguments
    {
        string Office365CustomerId { get; set; }
        string Domain { get; set; }
    }
}
