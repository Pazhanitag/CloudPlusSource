namespace CloudPlus.QueueModels.Office365.Domain.Commands
{
    public interface IOffice365ResendTxtRecordsCommand
    {
        int CompanyId { get; set; }
        string Domain { get; set; }
        string Email { get; set; }
        string Office365CustomerId { get; set; }
    }
}
