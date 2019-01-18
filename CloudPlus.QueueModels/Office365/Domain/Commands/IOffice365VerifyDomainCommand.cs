namespace CloudPlus.QueueModels.Office365.Domain.Commands
{
    public interface IOffice365VerifyDomainCommand
    {
        string DomainName { get; set; }
        string Office365CustomerId { get; set; }
    }
}
