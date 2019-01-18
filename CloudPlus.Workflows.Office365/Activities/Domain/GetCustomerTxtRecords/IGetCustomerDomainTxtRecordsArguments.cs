namespace CloudPlus.Workflows.Office365.Activities.Domain.GetCustomerTxtRecords
{
    public interface IGetCustomerDomainTxtRecordsArguments
    {
        string Office365CustomerId { get; set; }
        string Domain { get; set; }
    }
}
