using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer
{
    public interface ICreateDatabaseCustomerActivity : Activity<ICreateDatabaseCustomerArguments, ICreateDatabaseCustomerLog>
    {
    }
}