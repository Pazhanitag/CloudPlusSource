using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class ReceiveObserver : IReceiveObserver
    {
        public Task PreReceive(ReceiveContext context)
        {
            return Task.FromResult(context);
        }

        public Task PostReceive(ReceiveContext context)
        {
            return Task.FromResult(context);
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
            where T : class
        {
            return Task.FromResult(context);
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan elapsed, string consumerType,
            Exception exception) where T : class
        {
            return Task.FromResult(context);
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            return Task.FromResult(context);
        }
    }
}