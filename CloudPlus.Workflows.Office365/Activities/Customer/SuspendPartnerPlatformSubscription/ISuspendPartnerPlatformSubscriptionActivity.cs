using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.SuspendPartnerPlatformSubscription
{
    public interface ISuspendPartnerPlatformSubscriptionActivity : Activity<ISuspendPartnerPlatformSubscriptionArguments
        , ISuspendPartnerPlatformSubscriptionLog>
    {

    }
}