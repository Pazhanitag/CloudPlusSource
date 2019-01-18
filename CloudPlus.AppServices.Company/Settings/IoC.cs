using Autofac;
using Autofac.Extras.DynamicProxy;
using CloudPlus.AppServices.Company.AutofacModules;
using CloudPlus.AppServices.Company.Consumers;
using CloudPlus.AppServices.Company.Workflow.Builder;
using CloudPlus.AppServices.Company.Workflow.CreateCompany;
using CloudPlus.Database;
using CloudPlus.Database.Authentication;
using CloudPlus.DynamicProxy.Interceptors.Loggers;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.ProductConstraints;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Services.Database.WorkflowActivity.Company;
using CloudPlus.Services.Identity.Client;
using CloudPlus.Services.Identity.Password;
using CloudPlus.Services.Identity.User;
using CloudPlus.Settings;
using CloudPlus.Workflows.Common.ActivityConfigurator;
using CloudPlus.Workflows.Common.Consumers;
using CloudPlus.Workflows.Common.Workflow;
using CloudPlus.Workflows.Company.Activities.AddCallbackRedirectUri;
using CloudPlus.Workflows.Company.Activities.AssignCatalog;
using CloudPlus.Workflows.Company.Activities.CompanyCreated;
using CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany;
using CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany;
using CloudPlus.Workflows.Company.Activities.RemoveCallbackRedirectUri;
using CloudPlus.Workflows.Company.Mappers;
using CloudPlus.Workflows.User.Activities.CreateActiveDirectoryUser;
using CloudPlus.Workflows.User.Activities.CreateIdentityServerUser;
using CloudPlus.Workflows.User.Mappers;

namespace CloudPlus.AppServices.Company.Settings
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

            builder.RegisterType<CompanyProvisioningService>();
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<CompanyServiceSettings>().As(typeof(ICompanyServiceSettings), typeof(IRabbitMqSettings));


            builder.RegisterType<JsonValueSqlBuilder>().As<IJsonValueSqlBuilder>();
            builder.RegisterType<JsonSerializer>().As<IObjectSerializer>();
            builder.RegisterType<HttpClientResolver>().As<IHttpClientResolver>();

            builder.RegisterType<WorkflowActivityConsumer>().As<IWorkflowActivityConsumer>();
            builder.RegisterType<RoutingSlipStartedConsumer>().As<IRoutingSlipStartedConsumer>();

            builder.RegisterType<CldpDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerDependency();

            builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<CreateCompanyConsumer>().As<ICreateCompanyConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));
            builder.RegisterType<UpdateCompanyConsumer>().As<IUpdateCompanyConsumer>().EnableInterfaceInterceptors().InterceptedBy(typeof(IConsumerInterceptorLogger));

            builder.RegisterType<CompanyWorkflowBuilder>().As<IWorkflowBuilder>();
            builder.RegisterType<ActivityConfigurator>().As<IActivityConfigurator>();

            builder.RegisterType<CreateCompanyWorkflow>().As<ICreateCompanyWorkflow>().EnableInterfaceInterceptors().InterceptedBy(typeof(IInterceptorLogger));

            builder.RegisterType<ActivityCompanyArgumentsMapper>().As<IActivityCompanyArgumentsMapper>();
            builder.RegisterType<ActivityUserArgumentsMapper>().As<IActivityUserArgumentsMapper>();

            builder.RegisterType<PasswordService>().As<IPasswordService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyService>().As<ICompanyService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<UserService>().As<IUserService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<DomainService>().As<IDomainService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<WorkflowActivityService>().As<IWorkflowActivityService>();
            builder.RegisterType<WorkflowCompanyActivityService>().As<IWorkflowCompanyActivityService>();
            builder.RegisterType<WorkflowUserActivityService>().As<IWorkflowUserActivityService>();
            builder.RegisterType<ProductConstraintsService>().As<IProductConstraintsService>().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CompanyCatalogService>().As<ICompanyCatalogService>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogProductItemService>().As<ICatalogProductItemService>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CatalogUtilities>().As<ICatalogUtilities>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<CustomerCatalogService>().As<ICustomerCatalogService>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            builder.RegisterType<ClientService>().As<IClientService>().InstancePerLifetimeScope().EnableInterfaceInterceptors().InterceptedBy(typeof(IServiceInterceptorLogger));
            
            builder.RegisterType<CreateActiveDirectoryComapnyActivity>().As<ICreateActiveDirectoryComapnyActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateDatabaseCompanyActivity>().As<ICreateDatabaseCompanyActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateActiveDirectoryUserActivity>().As<ICreateActiveDirectoryUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CreateIdentityServerUserActivity>().As<ICreateIdentityServerUserActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<CompanyCreatedActivity>().As<ICompanyCreatedActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AssignCatalogActivity>().As<IAssignCatalogActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<AddCallbackRedirectUriActivity>().As<IAddCallbackRedirectUriActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
            builder.RegisterType<RemoveCallbackRedirectUriActivity>().As<IRemoveCallbackRedirectUriActivity>().EnableInterfaceInterceptors().InterceptedBy(typeof(IActivityInterceptorLogger));
        }
    }
}
