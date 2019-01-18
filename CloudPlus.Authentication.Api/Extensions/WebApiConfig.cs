using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using CloudPlus.Infrastructure.Http.Handlers;

namespace CloudPlus.Authentication.Api.Extensions
{
    public static class WebApiConfig
    {
        public static IAppBuilder ConfigureWebApi(this IAppBuilder app, HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.MessageHandlers.Add(new ResponseMessageHandler());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi",
                "{controller}/{id}", new
                {
                    id = RouteParameter.Optional
                });

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.EnsureInitialized();

            app.UseWebApi(config);

            return app;
        }
    }
}