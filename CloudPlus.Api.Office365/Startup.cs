using System.Web.Http;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using CloudPlus.Api.Office365;
using CloudPlus.Api.Office365.Extensions;
using CloudPlus.Logging;

[assembly: OwinStartup(typeof(Startup))]

namespace CloudPlus.Api.Office365
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
            app.UseWebApi(config);

            this.Log().Info($"Started {GetType().Namespace}");
        }
    }
}
