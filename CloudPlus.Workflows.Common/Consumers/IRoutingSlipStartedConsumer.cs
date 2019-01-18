using MassTransit;
using CloudPlus.QueueModels.Common;

namespace CloudPlus.Workflows.Common.Consumers
{
    public interface IRoutingSlipStartedConsumer : IConsumer<IRoutingSlipStarted>
    {
    }
}