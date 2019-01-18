using Autofac;
using MassTransit;
using CloudPlus.Constants;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Workflows.Common.Workflow;

namespace CloudPlus.AppServices.User.AutofacModules
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

                        busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.QueueCreateUser,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.QueueUpdateUser,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.QueueChangeUserPassword,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.RoutingSlipEventObserverRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));

	                    busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.QueueDeleteUser,
		                    endpointConfigurator => endpointConfigurator.LoadFrom(context));

                        busFactoryConfigurator.ReceiveEndpoint(host, UserServiceConstants.RoutingSlipUserStartedEventRoute,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));
                    });
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}
