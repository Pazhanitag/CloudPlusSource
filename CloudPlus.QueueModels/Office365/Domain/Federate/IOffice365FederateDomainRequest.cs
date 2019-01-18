namespace CloudPlus.QueueModels.Office365.Domain.Federate
{
    public interface IOffice365FederateDomainRequest
    {
        int CompanyId { get; set; }
        string DomainName { get; set; }
    }
}
