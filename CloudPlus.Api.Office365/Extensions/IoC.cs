using System.Reflection;
using System.Web.Http;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;
using CloudPlus.Api.Office365.Utils;
using CloudPlus.PowerShell;
using CloudPlus.Resources;

namespace CloudPlus.Api.Office365.Extensions
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
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>().InstancePerRequest();
            builder.RegisterType<PowerShellManager>().As<IPowerShellManager>().InstancePerRequest();
            builder.RegisterType<PowershellScriptLoader>().As<IPowershellScriptLoader>().InstancePerRequest();
            builder.RegisterType<PowerShellUtility>().As<IPowerShellUtility>().InstancePerRequest();
        }
    }
}
