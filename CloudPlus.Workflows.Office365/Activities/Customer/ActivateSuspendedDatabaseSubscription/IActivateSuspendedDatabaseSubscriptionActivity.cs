using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.ActivateSuspendedDatabaseSubscription
{
    public interface
        IActivateSuspendedDatabaseSubscriptionActivity : Activity<IActivateSuspendedDatabaseSubscriptionArguments,
            IActivateSuspendedDatabaseSubscriptionLog>
    {

    }
}