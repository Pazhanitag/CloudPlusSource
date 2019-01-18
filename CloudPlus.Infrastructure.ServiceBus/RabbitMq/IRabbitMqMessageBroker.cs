using System;
using CloudPlus.Infrastructure.ServiceBus.Configuration;
using MassTransit.RabbitMqTransport;

namespace CloudPlus.Infrastructure.ServiceBus.RabbitMq
{
    public interface IRabbitMqMessageBroker
    {
        IMessageBroker ConfigureBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> rabbitMqBusFactoryAction,
            Action<IAddLogAction> logAction = null);
    }
}