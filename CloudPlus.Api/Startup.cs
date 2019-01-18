using System.Security.Claims;
using System.Web.Http;
using CloudPlus.Api;
using CloudPlus.Api.Extensions;
using CloudPlus.Logging;
using CloudPlus.Resources;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CloudPlus.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();

            this.Log().Info($"Starting {GetType().Namespace}");

            var configurationManager = new ConfigurationManager();
            var config = new HttpConfiguration();

            app.UseCors(CorsOptions.AllowAll);

            app.UseIoC(config);

            app.ConfigureWebApi(config);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = configurationManager.GetByKey("CloudPlus.IdentityServerEndpoint"),
                ClientId = configurationManager.GetByKey("CloudPlus.PortalClientId"),
                ClientSecret = configurationManager.GetByKey("CloudPlus.PortalClientSecret"),
                RequiredScopes = new[] { "write", "read" },
                NameClaimType = ClaimTypes.NameIdentifier,
                DelayLoadMetadata = true
            });

            app.UseOpenApiSpecification(config);

            app.UseMessageBroker();

            app.UseWebApi(config);

            this.Log().Info($"Started {GetType().Namespace}");
        }
    }
}