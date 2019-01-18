using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiPartnerPlatformCustomerSubscription
{
    public interface IMultiPartnerPlatformCustomerSubscriptionActivity 
        : Activity<IMultiPartnerPlatformCustomerSubscriptionArguments, IMultiPartnerPlatformCustomerSubscriptionLog>
    {
    }
}
