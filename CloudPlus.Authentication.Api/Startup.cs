using CloudPlus.Authentication.Api.Extensions;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using CloudPlus.Logging;

namespace CloudPlus.Authentication.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();

            this.Log().Info($"Starting {GetType().Namespace}");

            var httpConfig = new HttpConfiguration();

            app.Map("/cloudplus", cloudPlus =>
            {
                cloudPlus.UseCors(CorsOptions.AllowAll);

                cloudPlus.UseIoC(httpConfig);

                cloudPlus.UseIdentityServerAuthFlow(httpConfig);

                cloudPlus.UseBearerTokenAuthentication();

                cloudPlus.UseOpenApiSpecification(httpConfig);

                cloudPlus.ConfigureWebApi(httpConfig);
            });

            this.Log().Info($"Started {GetType().Namespace}");
        }
    }
}