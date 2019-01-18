using Autofac;
using CloudPlus.Api.NotificationService.Consumers;
using CloudPlus.Api.NotificationService.Services;
using CloudPlus.Infrastructure.Email;
using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Services.Database.EmailNotifications;
using MassTransit;

namespace CloudPlus.Api.NotificationService.AutofacModules
{
    public class MessageBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    var messageBroker = context.Resolve<IRabbitMqMessageBroker>();
                    var emailMessageService = context.Resolve<IEmailMessageService>();
                    var sendEmailService = context.Resolve<ISendEmailService>();

                    return messageBroker.ConfigureBus((busFactoryConfigurator, host) => busFactoryConfigurator.ReceiveEndpoint(host, "send-email-notification_flow",
                        endpointConfigurator => endpointConfigurator.Consumer(
                            () => new SendEmailUserConsumer(emailMessageService, sendEmailService))), logger =>
                    {
                        //logger.AddLog(new PublishObserver());
                    });
                })
                .SingleInstance()
                .As<IMessageBroker>();
        }
    }
}