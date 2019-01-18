namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription
{
    public interface IDecreasePartnerPlatformCustomerSubscriptionArguments
    {
        int CompanyId { get; set; }
        string CloudPlusProductIdentifier { get; set; }
    }
}
