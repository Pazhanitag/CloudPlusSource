using System.Linq;
using System.Web.Http;
using Owin;
using Swashbuckle.Application;

namespace CloudPlus.Api.ActiveDirectory.Extensions
{
    public static class SwaggerConfig
    {
        public static void UseOpenApiSpecification(this IAppBuilder app, HttpConfiguration config)
        {
            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "CloudPlus")
                        .Description("CloudPlus Active Directory Service");
                    c.ResolveConflictingActions(x => x.First());
                })
                .EnableSwaggerUi();
        }
    }
}