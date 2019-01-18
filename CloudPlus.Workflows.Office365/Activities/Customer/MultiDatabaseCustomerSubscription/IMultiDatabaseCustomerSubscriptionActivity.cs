using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.MultiDatabaseCustomerSubscription
{
    public interface IMultiDatabaseCustomerSubscriptionActivity
        : Activity<IMultiDatabaseCustomerSubscriptionArguments, IMultiDatabaseCustomerSubscriptionLog>
    {
    }
}
