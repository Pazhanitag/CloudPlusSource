using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Logging;

namespace CloudPlus.AppServices.Notification
{
    public class NotificationProvisioningService
    {
        private readonly IMessageBroker _bus;

        public NotificationProvisioningService(IMessageBroker bus)
        {
            _bus = bus;
        }

        public void Start()
        {
            this.Log().Info($"Starting {GetType().Name}");

            _bus.Start();

            this.Log().Info($"{GetType().Name} started");
        }

        public void Stop()
        {
            this.Log().Info($"Stopping {GetType().Name}");

            _bus?.Stop();

            this.Log().Info($"{GetType().Name} stopped");
        }
    }
}
