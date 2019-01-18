using Autofac;
using Autofac.Extras.DynamicProxy;
using CloudPlus.AppServices.Notification.AutofacModules;
using CloudPlus.AppServices.Notification.Consumers;
using CloudPlus.AppServices.Notification.Services;
using CloudPlus.Database;
using CloudPlus.Database.Authentication;
using CloudPlus.DynamicProxy.Interceptors.Loggers;
using CloudPlus.Infrastructure.Email;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.EmailNotifications;
using CloudPlus.Services.Database.Support;
using CloudPlus.Services.Identity.User;
using CloudPlus.Settings;

namespace CloudPlus.AppServices.Notification.Settings
{
    public static class IoC
    {
        private static IContainer _container;

        internal static IContainer SetupAutofacContainer()
        {
            var builder = new ContainerBuilder();

            RegisterDependencies(builder);

            _container = builder.Build();

            return _container;
        }

        public static IContainer GetContainer()
        {
            return _container;
        }

        private static void RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<InterceptorLogger>().As<IInterceptorLogger>();
            builder.RegisterType<ServiceInterceptorLogger>().As<IServiceInterceptorLogger>();
            builder.RegisterType<ConsumerInterceptorLogger>().As<IConsumerInterceptorLogger>();

            builder.RegisterType<NotificationProvisioningService>();
            builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();
            builder.RegisterType<CldpDbContext>().InstancePerDependency();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerDependency();
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<NotificationServiceSettings>().As(typeof(INotificationServiceSettings), typeof(IRabbitMqSettings));
            builder.RegisterType<SendEmailUserConsumer>().As<ISendEmailUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<SmtpClientResolver>().As<ISmtpClientResolver>().InstancePerDependency().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<SmtpManager>().As<ISmtpManager>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerDependency().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyCatalogService>().As<ICompanyCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger)); 
            builder.RegisterType<CatalogProductItemService>().As<ICatalogProductItemService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger)); 
            builder.RegisterType<CatalogUtilities>().As<ICatalogUtilities>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CustomerCatalogService>().As<ICustomerCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger)); 
            builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<SendEmailService>().As<ISendEmailService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<EmailMessageService>().As<IEmailMessageService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CustomSecureControlPanelService>().As<ICustomSecureControlPanelService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
        }
    }
}
