using Autofac;
using CloudPlus.Resources;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace CloudPlus.Api.NotificationService.Extensions
{
    public static class SignalR
    {
        public static void UseSignalR(this IAppBuilder app)
        {
            var configurationManager = IoC.GetContainer().Resolve<IConfigurationManager>();

            app.Map(configurationManager.GetByKey("SignalREndpoint"), map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration();

                map.RunSignalR(hubConfiguration);
            });
        }
    }
}