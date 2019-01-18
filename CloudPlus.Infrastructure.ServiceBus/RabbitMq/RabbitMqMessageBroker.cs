using System;
using CloudPlus.Infrastructure.ServiceBus.Configuration;
using CloudPlus.Infrastructure.ServiceBus.Encryption;
using CloudPlus.Settings;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.QuartzIntegration;
using MassTransit.RabbitMqTransport;
using MassTransit.Serialization;
using Quartz;
using Quartz.Impl;

namespace CloudPlus.Infrastructure.ServiceBus.RabbitMq
{
    public class RabbitMqMessageBroker : IRabbitMqMessageBroker
    {
        private readonly IRabbitMqSettings _settings;
        private readonly IScheduler _scheduler;

        public RabbitMqMessageBroker(IRabbitMqSettings settings)
        {
            _settings = settings;
            _scheduler = CreateScheduler();
        }

        public IMessageBroker ConfigureBus(
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> rabbitMqBusFactoryAction,
            Action<IAddLogAction> logAction = null) 
        {
            if (rabbitMqBusFactoryAction == null)
                throw new ArgumentNullException();

            var rabbitMqUri = _settings.RabbitMqUri;
            var rabbitMqUsername = _settings.RabbitMqUserName;
            var rabbitMqPassword = _settings.RabbitMqPassword;

            if (string.IsNullOrWhiteSpace(rabbitMqUri) || string.IsNullOrWhiteSpace(rabbitMqUsername) ||
                string.IsNullOrWhiteSpace(rabbitMqPassword))
                throw new Exception("Invalid RabbitMq configuration!");

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(rabbitMqUri), h =>
                {
                    h.Username(rabbitMqUsername);
                    h.Password(rabbitMqPassword);
                });
                cfg.Durable = true;

#if DEBUG
                cfg.UseLog4Net();
#endif

//#if RELEASE
//                cfg.UseEncryptedSerializer(new AesCryptoStreamProvider(new CldpKeyProvider(), "default"));
//#endif
                
                cfg.ReceiveEndpoint("quartz", e =>
                {
                    cfg.UseMessageScheduler(e.InputAddress);

                    e.Consumer(() => new ScheduleMessageConsumer(_scheduler));
                    e.Consumer(() => new CancelScheduledMessageConsumer(_scheduler));
                });
                rabbitMqBusFactoryAction.Invoke(cfg, host);
            });

            logAction?.Invoke(new AddLogAction(busControl));

            return new MessageBroker(busControl, rabbitMqUri, _scheduler);
        }
        static IScheduler CreateScheduler()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            return schedulerFactory.GetScheduler();
        }
    }
}