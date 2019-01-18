namespace CloudPlus.Workflows.Office365.Activities.Domain.AddCustomerDomainPartnerPortalActivity
{
    public interface IAddCustomerDomainPartnerPortalArguments
    {
        string Office365CustomerId { get; set; }
        string Domain { get; set; }
    }
}
