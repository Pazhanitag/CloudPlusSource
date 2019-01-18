namespace CloudPlus.QueueModels.Office365.Domain.Federate
{
    public class Office365FederateDomainRequest : IOffice365FederateDomainRequest
    {
        public int CompanyId { get; set; }
        public string DomainName { get; set; }
    }
}
