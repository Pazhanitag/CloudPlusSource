using Autofac;
using MassTransit;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Constants;

namespace CloudPlus.AppServices.Notification.AutofacModules
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var messageBroker = context.Resolve<IRabbitMqMessageBroker>();

                    return messageBroker.ConfigureBus((busFactoryConfigurator, host) =>
                    {
                        busFactoryConfigurator.ReceiveEndpoint(host, NotificationServiceConstants.QueueSendEmailNotification,
                            endpointConfigurator => endpointConfigurator.LoadFrom(context));
                    });
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}
