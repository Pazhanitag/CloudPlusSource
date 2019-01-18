using System.Web.Http;
using Owin;
using Newtonsoft.Json.Serialization;
using CloudPlus.Infrastructure.Http.Handlers;

namespace CloudPlus.Api.Office365.Extensions
{
    public static class WebApiConfig
    {
        public static void ConfigureWebApi(this IAppBuilder app, HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new ResponseMessageHandler());
            config.MessageHandlers.Add(new LoggingHandler());
            config.Routes.MapHttpRoute("DefaultApi",
                "{controller}/{id}", new
                {
                    id = RouteParameter.Optional
                });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.EnsureInitialized();
        }
    }
}
