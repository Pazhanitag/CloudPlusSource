using System;
using System.Threading;
using Autofac;
using CloudPlus.Infrastructure.ServiceBus;
using Microsoft.Owin;
using Owin;

namespace CloudPlus.Api.Extensions
{
    public static class ServiceBus
    {
        public static void UseMessageBroker(this IAppBuilder app)
        {
            var messageBroker = IoC.GetContainer().Resolve<IMessageBroker>();

            if (messageBroker == null)
                throw new Exception("Invalid Autofac MssageBroker instance");

            messageBroker.Start();

            if (!app.Properties.ContainsKey("host.OnAppDisposing"))
                return;

            var context = new OwinContext(app.Properties);
            var token = context.Get<CancellationToken>("host.OnAppDisposing");

            if (token != CancellationToken.None)
                token.Register(() => messageBroker.Stop());
        }
    }
}