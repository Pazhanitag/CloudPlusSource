namespace CloudPlus.Workflows.Office365.Activities.Domain.SendCustomerTxtRecords
{
    public interface ISendCustomerDomainTxtRecordsArguments
    {
        int CompanyId { get; set; }
        string Email { get; set; }
        string TxtRecords { get; set; }
        string Domain { get; set; }
    }
}
