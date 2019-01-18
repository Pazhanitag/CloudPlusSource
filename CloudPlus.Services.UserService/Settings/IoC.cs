using Autofac;
using Autofac.Extras.DynamicProxy;
using CloudPlus.AppServices.User.AutofacModules;
using CloudPlus.AppServices.User.Consumers;
using CloudPlus.AppServices.User.Workflow.Builder;
using CloudPlus.AppServices.User.Workflow.CreateUser;
using CloudPlus.AppServices.User.Workflow.DeleteUser;
using CloudPlus.AppServices.User.Workflow.UpdateUser;
using CloudPlus.Database;
using CloudPlus.Database.Authentication;
using CloudPlus.DynamicProxy.Interceptors.Loggers;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Identity.Password;
using CloudPlus.Services.Identity.User;
using CloudPlus.Settings;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Consumers;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;
using CloudPlus.Workflows.User.Activities.DeleteActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.DeleteIdentityServerUser;
using CloudPlus.Workflows.User.Activities.UpdateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.UpdateIdentityServerUser;
using CloudPlus.Workflows.User.Activities.UserCreated;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.User.Settings
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
            builder.RegisterType<InterceptorLogger>().As<IInterceptorLogger>();
            builder.RegisterType<ServiceInterceptorLogger>().As<IServiceInterceptorLogger>();
            builder.RegisterType<ConsumerInterceptorLogger>().As<IConsumerInterceptorLogger>();
            builder.RegisterType<ActivityInterceptorLogger>().As<IActivityInterceptorLogger>();

            
            builder.RegisterType<UserProvisioningService>();
            builder.RegisterType<UserServiceSettings>().As(typeof(IUserServiceSettings), typeof(IRabbitMqSettings));

            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<HttpClientResolver>().As<IHttpClientResolver>();
            builder.RegisterType<JsonValueSqlBuilder>().As<IJsonValueSqlBuilder>();
            builder.RegisterType<JsonSerializer>().As<IObjectSerializer>();

            builder.RegisterType<WorkflowActivityConsumer>().As<IWorkflowActivityConsumer>();
            builder.RegisterType<RoutingSlipStartedConsumer>().As<IRoutingSlipStartedConsumer>();

            builder.RegisterType<CldpDbContext>().InstancePerDependency();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerDependency();

            builder.RegisterType<CreateUserConsumer>().As<ICreateUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<UpdateUserConsumer>().As<IUpdateUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<ChangeUserPasswordConsumer>().As<IChangeUserPasswordConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
	        builder.RegisterType<DeleteUserConsumer>().As<IDeleteUserConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));

            builder.RegisterType<UserWorkflowBuilder>().As<IWorkflowBuilder>();
            builder.RegisterType<ActivityConfigurator>().As<IActivityConfigurator>();

            builder.RegisterType<CreateUserWorkflow>().As<ICreateUserWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
            builder.RegisterType<UpdateUserWorkflow>().As<IUpdateUserWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));
	        builder.RegisterType<DeleteUserWorkflow>().As<IDeleteUserWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));

			builder.RegisterType<ActivityUserArgumentsMapper>().As<IActivityUserArgumentsMapper>();
	        builder.RegisterType<ActivityDeleteUserArgumentsMapper>().As<IActivityDeleteUserArgumentsMapper>();

			builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<CompanyService>().As<ICompanyService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<UserService>().As<IUserService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<PasswordService>().As<IPasswordService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyCatalogService>().As<ICompanyCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogProductItemService>().As<ICatalogProductItemService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogUtilities>().As<ICatalogUtilities>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CustomerCatalogService>().As<ICustomerCatalogService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));

            builder.RegisterType<WorkflowActivityService>().As<IWorkflowActivityService>();
            builder.RegisterType<WorkflowUserActivityService>().As<IWorkflowUserActivityService>();

            builder.RegisterType<CreateActiveDirectoryUserActivity>().As<ICreateActiveDirectoryUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateIdentityServerUserActivity>().As<ICreateIdentityServerUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateActiveDirectoryUserActivity>().As<IUpdateActiveDirectoryUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UpdateIdentityServerUserActivity>().As<IUpdateIdentityServerUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<UserCreatedActivity>().As<IUserCreatedActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
			builder.RegisterType<DeleteActiveDirectoryUserActivity>().As<IDeleteActiveDirectoryUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
			builder.RegisterType<DeleteIdentityServerUserActivity>().As<IDeleteIdentityServerUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
		}
    }
}