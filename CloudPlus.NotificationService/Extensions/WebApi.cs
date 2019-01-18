using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Owin;

namespace CloudPlus.Api.NotificationService.Extensions
{
    public static class WebApi
    {
        public static void ConfigureWebApi(this IAppBuilder app, HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.EnsureInitialized();
        }
    }
}