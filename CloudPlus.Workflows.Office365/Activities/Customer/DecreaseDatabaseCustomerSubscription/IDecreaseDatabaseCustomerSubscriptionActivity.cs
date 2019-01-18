using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.DecreaseDatabaseCustomerSubscription
{
    public interface IDecreaseDatabaseCustomerSubscriptionActivity 
        : Activity<IDecreaseDatabaseCustomerSubscriptionArguments, IDecreaseDatabaseCustomerSubscriptionLog>
    {
    }
}
