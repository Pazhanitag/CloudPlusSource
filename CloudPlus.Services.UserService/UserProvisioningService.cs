using CloudPlus.Infrastructure.ServiceBus;
using CloudPlus.Logging;

namespace CloudPlus.AppServices.User
{
    public class UserProvisioningService
    {
        private readonly IMessageBroker _bus;

        public UserProvisioningService(IMessageBroker bus)
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