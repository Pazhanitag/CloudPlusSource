using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class SendObserver : ISendObserver
    {
        public Task PreSend<T>(SendContext<T> context)
            where T : class
        {
            return Task.FromResult(context);
        }

        public Task PostSend<T>(SendContext<T> context)
            where T : class
        {
            return Task.FromResult(context);
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception)
            where T : class
        {
            return Task.FromResult(context);
        }
    }
}