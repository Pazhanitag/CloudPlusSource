using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using CloudPlus.Api.Attributes;
using CloudPlus.Infrastructure.Http.Handlers;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

namespace CloudPlus.Api.Extensions
{
    public static class WebApi
    {
        public static void ConfigureWebApi(this IAppBuilder app, HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            

            config.MessageHandlers.Add(new ResponseMessageHandler());
            config.MessageHandlers.Add(new LoggingHandler());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            config.Filters.Add(new ExceptionHandlingAttribute());
            config.Filters.Add(new InterceptionAttribute());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = false,
                EnableDefaultFiles = true
            };

            app.UseFileServer(options);

            config.EnsureInitialized();
        }
    }
}