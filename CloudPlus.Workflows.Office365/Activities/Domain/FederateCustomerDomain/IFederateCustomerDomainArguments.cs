namespace CloudPlus.Workflows.Office365.Activities.Domain.FederateCustomerDomain
{
    public interface IFederateCustomerDomainArguments
    {
        string AdminUserName { get; set; }
        string AdminPassword { get; set; }
        string Office365CustomerId { get; set; }
        string DomainName { get; set; }
    }
}