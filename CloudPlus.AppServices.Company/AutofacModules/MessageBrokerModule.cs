using Autofac;
using MassTransit;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.Company.AutofacModules
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var messageBroker = context.Resolve<IRabbitMqMessageBroker>();
                    var workflowBuilder = context.Resolve<IWorkflowBuilder>();

                    return messageBroker.ConfigureBus((busFactoryConfigurator, host) =>
                    {
                        workflowBuilder.BuildWorkflow(busFactoryConfigurator, host, context.ComponentRegistry);
                        
                        busFactoryConfigurator.ReceiveEndpoint(host, CompanyServiceConstants.QueueCreateCompany,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, CompanyServiceConstants.QueueUpdateCompany,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, CompanyServiceConstants.RoutingSlipEventObserverRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, CompanyServiceConstants.RoutingSlipCompanyStartedEventRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));
                    });
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}
