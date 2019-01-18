using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CloudPlus.Api.AutofacModules;
using CloudPlus.Api.Helpers;
using CloudPlus.Api.Security;
using CloudPlus.Api.Settings;
using CloudPlus.Database;
using CloudPlus.Database.Authentication;
using CloudPlus.Infrastructure.Cache;
using CloudPlus.Infrastructure.Email;
using CloudPlus.Infrastructure.Http;
using CloudPlus.Infrastructure.ServiceBus.RabbitMq;
using CloudPlus.Resources;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.Database.Catalog;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.Office365.Api;
using CloudPlus.Services.Database.ProductConstraints;
using CloudPlus.Services.Database.WorkflowActivity;
using CloudPlus.Settings;
using Microsoft.Owin;
using Owin;
using CloudPlus.Services.Database.WorkflowActivity.Company;
using CloudPlus.Services.Identity.Password;
using CloudPlus.Services.Identity.Permission;
using CloudPlus.Services.Identity.Role;
using CloudPlus.Services.Identity.User;
using CloudPlus.Services.Database.WorkflowActivity.Office365;
using CloudPlus.Services.Database.Office365.Customer;
using CloudPlus.Services.Database.Office365.Domain;
using CloudPlus.Services.Database.Office365.License;
using CloudPlus.Services.Database.Office365.Offer;
using CloudPlus.Services.Database.Office365.Utilities;
using CloudPlus.Services.Database.Provisions;
using CloudPlus.Services.Database.Office365.Role;
using CloudPlus.Services.Database.Office365.User;
using CloudPlus.Services.Database.Product;
using CloudPlus.Services.Identity.Client;
using CloudPlus.Services.Office365.Transition;
using CloudPlus.Services.Database.Metrics;
using CloudPlus.Services.Database.Support;

namespace CloudPlus.Api.Extensions
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
            builder.RegisterType<HttpClientResolver>().As<IHttpClientResolver>();
            builder.RegisterType<SmtpClientResolver>().As<ISmtpClientResolver>();
            builder.RegisterType<SmtpManager>().As<ISmtpManager>();
            builder.RegisterType<AuthorizationManager>().As<IAuthorizationManager>();
            builder.RegisterType<InMemoryCacheStore>().As<ICacheStore>();
            builder.RegisterType<JsonSerializer>().As<IObjectSerializer>();
            builder.RegisterType<CacheStoreResolver>().As<ICacheStoreResolver>();
            builder.RegisterType<ImageHelper>().As<IImageHelper>();
            builder.RegisterType<CatalogHelper>().As<ICatalogHelper>();

            builder.RegisterType<PasswordService>().As<IPasswordService>();
            builder.RegisterType<CloudPlusApiSettings>().As(typeof(IRabbitMqSettings), typeof(ICloudPlusApiSettings));
            builder.RegisterType<CldpDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<ActiveDirectoryService>().As<IActiveDirectoryService>();
            builder.RegisterType<CompanyService>().As<ICompanyService>();
            builder.RegisterType<DomainService>().As<IDomainService>();
            builder.RegisterType<Office365DbDomainService>().As<IOffice365DbDomainService>();
            builder.RegisterType<Office365DbUtilitiesService>().As<IOffice365DbUtilitiesService>();
            builder.RegisterType<Office365DbCustomerService>().As<IOffice365DbCustomerService>();
            builder.RegisterType<Office365DbUserService>().As<IOffice365DbUserService>();
            builder.RegisterType<Office365DbLicenseService>().As<IOffice365DbLicenseService>();
            builder.RegisterType<Office356DbOfferService>().As<IOffice356DbOfferService>();
            builder.RegisterType<Office365DbRoleService>().As<IOffice365DbRoleService>();
            builder.RegisterType<RabbitMqMessageBroker>().As<IRabbitMqMessageBroker>();
            builder.RegisterModule<MessageBrokerModule>();

            builder.RegisterType<WorkflowUserActivityService>().As<IWorkflowUserActivityService>();
            builder.RegisterType<WorkflowCompanyActivityService>().As<IWorkflowCompanyActivityService>();
            builder.RegisterType<WorkflowOffice365ActivityService>().As<IWorkflowOffice365ActivityService>();
            builder.RegisterType<JsonValueSqlBuilder>().As<IJsonValueSqlBuilder>();
            builder.RegisterType<WorkflowActivityService>().As<IWorkflowActivityService>().InstancePerDependency();
            builder.RegisterType<JsonSerializer>().As<IObjectSerializer>();

            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<ProductConstraintsService>().As<IProductConstraintsService>();
            builder.RegisterType<CompanyCatalogService>().As<ICompanyCatalogService>();
            builder.RegisterType<CatalogProductItemService>().As<ICatalogProductItemService>();
            builder.RegisterType<CatalogUtilities>().As<ICatalogUtilities>();
            builder.RegisterType<CustomerCatalogService>().As<ICustomerCatalogService>();

            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PermissionService>().As<IPermissionService>();
            builder.RegisterType<RoleService>().As<IRoleService>();
            builder.RegisterType<Office365ApiService>().As<IOffice365ApiService>();
            builder.RegisterType<ProvisioningService>().As<IProvisioningService>();
            builder.RegisterType<Office365TransitionService>().As<IOffice365TransitionService>();
            builder.RegisterType<ClientService>().As<IClientService>();

            //Add by TAG
            builder.RegisterType<MetricsService>().As<IMetricsService>();
            builder.RegisterType<CustomSecureControlPanelService>().As<ICustomSecureControlPanelService>();
        }
    }
}