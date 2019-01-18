using Autofac.Core;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.Workflows.Common.Workflow
{
    public interface IWorkflowBuilder
    {
        void BuildWorkflow(IRabbitMqBusFactoryConfigurator busFactoryConfigurator, IRabbitMqHost host, IComponentRegistry componentRegistry);
    }
}
