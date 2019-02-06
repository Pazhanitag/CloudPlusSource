using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendMultiPartnerPlatformSubscription
{
    public interface ISuspendMultiPartnerPlatformSubscriptionActivity : Activity<ISuspendMultiPartnerPlatformSubscriptionArguments
        , ISuspendMultiPartnerPlatformSubscriptionLog>
    {
    }
}
