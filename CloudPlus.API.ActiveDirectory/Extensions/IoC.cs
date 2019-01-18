using System.Reflection;
using System.Web.Http;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;
using CloudPlus.Api.ActiveDirectory.Database;
using CloudPlus.Api.ActiveDirectory.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Resources;


namespace CloudPlus.Api.ActiveDirectory.Extensions
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
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>();
            builder.RegisterType<PowerShellManager>().As<IPowerShellManager>();
            builder.RegisterType<PowershellScriptLoader>().As<IPowershellScriptLoader>();
            builder.RegisterType<SamAccountNameGenerator>().As<ISamAccountNameGenerator>();
            builder.RegisterType<DatabaseManager>().As<IDatabaseManager>();
            builder.RegisterType<OrganizationalUnitRepository>().As<IOrganizationalUnitRepository>();
        }
    }
}
