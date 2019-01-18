using System.Web.Http;
using CloudPlus.Api.NotificationService.Extensions;
using CloudPlus.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(CloudPlus.Api.NotificationService.Startup))]

namespace CloudPlus.Api.NotificationService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.Log().Info($"Starting {GetType().Namespace}");

            var config = new HttpConfiguration();

            app.UseCors(CorsOptions.AllowAll);

            app.UseIoC(config);

            app.ConfigureWebApi(config);

            app.UseBearerTokenAuthentication();

            app.UseMessageBroker();

            app.UseWebApi(config);

            this.Log().Info($"Started {GetType().Namespace}");
        }
    }
}