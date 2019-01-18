using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class ConsumeMessageObserver<T> : IConsumeMessageObserver<T> where T : class
    {
        Task IConsumeMessageObserver<T>.PreConsume(ConsumeContext<T> context)
        {
            return Task.FromResult(context);
        }

        Task IConsumeMessageObserver<T>.PostConsume(ConsumeContext<T> context)
        {
            return Task.FromResult(context);
        }

        Task IConsumeMessageObserver<T>.ConsumeFault(ConsumeContext<T> context, Exception exception)
        {
            return Task.FromResult(context);
        }
    }
}