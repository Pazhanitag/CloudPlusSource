using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreasePartnerPlatformCustomerSubscription
{
    public interface IDecreasePartnerPlatformCustomerSubscriptionActivity 
        : Activity<IDecreasePartnerPlatformCustomerSubscriptionArguments, IDecreasePartnerPlatformCustomerSubscriptionLog>
    {
    }
}
