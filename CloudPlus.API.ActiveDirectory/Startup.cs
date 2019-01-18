using System.Web.Http;
using CloudPlus.Api.ActiveDirectory.Database;
using CloudPlus.Api.ActiveDirectory.Extensions;
using CloudPlus.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(CloudPlus.Api.ActiveDirectory.Startup))]

namespace CloudPlus.Api.ActiveDirectory
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();

            this.Log().Info($"Starting {GetType().Namespace}");

            var config = new HttpConfiguration();
            app.UseCors(CorsOptions.AllowAll);
            app.UseIoC(config);
            app.ConfigureWebApi(config);

            app.UseOpenApiSpecification(config);

            app.UseWebApi(config);

            app.InitializeDatabase(config.DependencyResolver.GetService(typeof(IDatabaseManager)) as IDatabaseManager);

            this.Log().Info($"Started {GetType().Namespace}");
        }
    }
}
