namespace CloudPlus.QueueModels.Office365.Domain.Commands
{
    public interface IOffice365AddAdditionalDomainCommand
    {
        string Office365CustomerId { get; set; }
        int CompanyId { get; set; }
        string Domain { get; set; }
        string Email { get; set; }
    }
}
