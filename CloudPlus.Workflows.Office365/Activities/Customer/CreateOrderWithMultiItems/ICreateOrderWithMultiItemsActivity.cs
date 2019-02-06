using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateOrderWithMultiItems
{
    public interface ICreateOrderWithMultiItemsActivity : Activity<ICreateOrderWithMultiItemsArguments, ICreateOrderWithMultiItemsLog>
    {
    }
}
