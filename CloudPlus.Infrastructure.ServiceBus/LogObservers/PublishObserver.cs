using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class PublishObserver : IPublishObserver
    {
        public Task PrePublish<T>(PublishContext<T> context)
            where T : class
        {
            return Task.FromResult(context);
        }

        public Task PostPublish<T>(PublishContext<T> context)
            where T : class
        {
            return Task.FromResult(context);
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception)
            where T : class
        {
            return Task.FromResult(context);
        }
    }
}