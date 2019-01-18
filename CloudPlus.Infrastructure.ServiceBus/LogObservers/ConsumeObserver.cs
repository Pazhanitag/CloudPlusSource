using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class ConsumeObserver : IConsumeObserver
    {
        Task IConsumeObserver.PreConsume<T>(ConsumeContext<T> context)
        {
            return Task.FromResult(context);
        }

        Task IConsumeObserver.PostConsume<T>(ConsumeContext<T> context)
        {
            return Task.FromResult(context);
        }

        Task IConsumeObserver.ConsumeFault<T>(ConsumeContext<T> context, Exception exception)
        {
            return Task.FromResult(context);
        }
    }
}