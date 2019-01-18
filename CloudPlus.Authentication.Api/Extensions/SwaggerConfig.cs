using System.Linq;
using System.Web.Http;
using Owin;
using Swashbuckle.Application;

namespace CloudPlus.Authentication.Api.Extensions
{
    public static class SwaggerConfig
    {
        public static void UseOpenApiSpecification(this IAppBuilder app, HttpConfiguration config)
        {
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "CloudPlus")
                    .Description("CloudPlus Authentication Service");
                c.ResolveConflictingActions(x => x.First());
                //c.OAuth2("oauth2")
                //    .Description("OAuth2 Password Grant")
                //    .Flow("password")
                //    .TokenUrl("http://localhost:62937/oauth/token")
                //    .Scopes(scopes => { });

            }).EnableSwaggerUi();
        }
    }
}