using Autofac;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;

namespace CloudPlus.Api.AutofacModules
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var messageBroker = context.Resolve<IRabbitMqMessageBroker>();

                    return messageBroker.ConfigureBus((busFactoryConfigurator, host) => { }, logger => { });
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}