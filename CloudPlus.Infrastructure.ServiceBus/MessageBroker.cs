using System;
using MassTransit;
using MassTransit.QuartzIntegration;
using MassTransit.Util;
using Quartz;

namespace CloudPlus.Infrastructure.ServiceBus
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IBusControl _busControl;
        private readonly string _rabbitMqUri;
        private BusHandle _busHandle;
        private readonly IScheduler _scheduler;

        public MessageBroker(IBusControl busControl, string rabbitMqUri, IScheduler scheduler)
        {
            _busControl = busControl;
            _rabbitMqUri = rabbitMqUri;
            _scheduler = scheduler;
        }

        public IBusControl Bus()
        {
            if (_busControl == null)
                throw new NullReferenceException();

            return _busControl;
        }

        public ISendEndpoint GetSendEndpoint(string queue)
        {
            var sendEndpointTask = Bus().GetSendEndpoint(new Uri(string.Concat(_rabbitMqUri, queue)));
            var sendEndpoint = sendEndpointTask.Result;

            return sendEndpoint;
        }

        public void Start()
        {
            try
            {
                _busHandle = TaskUtil.Await(() => _busControl.StartAsync());
                _scheduler.JobFactory = new MassTransitJobFactory(_busControl);

                _scheduler.Start();
            }
            catch (Exception)
            {
                _scheduler.Shutdown();
                throw;
            }
        }

        public void Stop()
        {
            if (_busHandle == null)
                throw new NullReferenceException();

            _scheduler.Standby();

            _busHandle.Stop(TimeSpan.FromSeconds(30));

            _scheduler.Shutdown();
        }

        public IRequestClient<TRequest, TResponse> GetRequestClient<TRequest, TResponse>(Uri endpoint, TimeSpan timeout) where TRequest: class where TResponse: class 
        {
            return _busControl.CreateRequestClient<TRequest, TResponse>(endpoint, timeout);
        }
    }
}