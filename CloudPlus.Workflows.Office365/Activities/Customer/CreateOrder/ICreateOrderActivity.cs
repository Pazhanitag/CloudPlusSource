using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrder
{
    public interface ICreateOrderActivity : Activity<ICreateOrderArguments, ICreateOrderLog>
    {

    }
}