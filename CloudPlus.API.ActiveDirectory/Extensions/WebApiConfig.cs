using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using CloudPlus.Infrastructure.Http.Handlers;
using Newtonsoft.Json.Serialization;
using Owin;

namespace CloudPlus.Api.ActiveDirectory.Extensions
{
    public static class WebApiConfig
    {
        public static void ConfigureWebApi(this IAppBuilder app, HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MessageHandlers.Add(new ResponseMessageHandler());
            config.MessageHandlers.Add(new LoggingHandler());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi",
                "{controller}/{id}", new
                {
                    id = RouteParameter.Optional
                });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.EnsureInitialized();

        }
    }
}