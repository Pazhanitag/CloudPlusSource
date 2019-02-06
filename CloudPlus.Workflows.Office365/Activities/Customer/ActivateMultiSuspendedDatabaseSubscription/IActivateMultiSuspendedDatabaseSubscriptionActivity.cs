

using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateMultiSuspendedDatabaseSubscription
{
    public interface IActivateMultiSuspendedDatabaseSubscriptionActivity : Activity<IActivateMultiSuspendedDatabaseSubscriptionArguments,
             IActivateMultiSuspendedDatabaseSubscriptionLog>
    {
    }
}
