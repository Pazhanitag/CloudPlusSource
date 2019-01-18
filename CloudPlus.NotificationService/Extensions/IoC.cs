using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CloudPlus.Api.NotificationService.AutofacModules;
using CloudPlus.Api.NotificationService.Services;
using CloudPlus.Api.NotificationService.Settings;
using CloudPlus.Infrastructure.Email;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.Database.EmailNotifications;
using CloudPlus.Settings;
using Microsoft.Owin;
using Owin;
using CloudPlus.Database;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Identity.User;
using CloudPlus.Database.Authentication;

namespace CloudPlus.Api.NotificationService.Extensions
{
    public static class IoC
    {
        private static IContainer _container;

        public static void UseIoC(this IAppBuilder app, HttpConfiguration config)
        {
            var autoFacConfig = SetupAutofacContainer();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(autoFacConfig);

            app.UseAutofacMiddleware(autoFacConfig);
            app.UseAutofacWebApi(config);
        }

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
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.Register(x => HttpContext.Current.GetOwinContext()).As<IOwinContext>();
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<SmtpClientResolver>().As<ISmtpClientResolver>();
            builder.RegisterType<SmtpManager>().As<ISmtpManager>();
            builder.RegisterType<NotificationServiceSettings>().As(typeof(IRabbitMqSettings), typeof(INotificationServiceSettings));
            //Service Bus configuration
            builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<CldpDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CompanyService>().As<ICompanyService>();
            builder.RegisterType<SendEmailService>().As<ISendEmailService>();
            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>();
            builder.RegisterType<EmailMessageService>().As<IEmailMessageService>();
        }
    }
}