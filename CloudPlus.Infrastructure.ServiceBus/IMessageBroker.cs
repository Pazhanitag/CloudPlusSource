using System;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus
{
    public interface IMessageBroker
    {
        IBusControl Bus();
        ISendEndpoint GetSendEndpoint(string queue);
        void Start();
        void Stop();
        IRequestClient<TRequest, TResponse> GetRequestClient<TRequest, TResponse>(Uri endpoint, TimeSpan timeout) where TRequest : class where TResponse : class;
    }
}