namespace CloudPlus.Workflows.Office365.Activities.Domain.VerifyCustomerDomain
{
    public interface IVerifyCustomerDomainArguments
    {
        string Office365CustomerId { get; set; }
        string DomainName { get; set; }
    }
}