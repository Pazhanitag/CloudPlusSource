using Autofac;
using Autofac.Integration.WebApi;
using CloudPlus.Authentication.Identity.Factory;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using System.Web;
using System.Web.Http;
using CloudPlus.Database.Authentication;
using CloudPlus.Resources;
using CloudPlus.Services.Identity.Permission;
using CloudPlus.Services.Identity.User;
using TokenProviderService = CloudPlus.Authentication.Identity.Services.TokenProviderService;
using ITokenProviderService = CloudPlus.Authentication.Identity.Services.ITokenProviderService;

namespace CloudPlus.Authentication.Api.Extensions
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
            builder.RegisterType<TokenProviderService>().As<ITokenProviderService>();
            builder.RegisterType<CloudPlusAuthDbContext>().InstancePerRequest();
            builder.RegisterType<IdentityFactory>().As<IIdentityFactory>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PermissionService>().As<IPermissionService>();
        }
    }
}