using MassTransit;
using MassTransit.Courier.Contracts;

namespace CloudPlus.Workflows.Common.Consumers
{
    public interface IWorkflowActivityConsumer :
        IConsumer<RoutingSlipActivityCompleted>,
        IConsumer<RoutingSlipActivityFaulted>,
        IConsumer<RoutingSlipActivityCompensated>,
        IConsumer<RoutingSlipActivityCompensationFailed>,
        IConsumer<RoutingSlipCompleted>,
        IConsumer<RoutingSlipFaulted>
    {
    }
}