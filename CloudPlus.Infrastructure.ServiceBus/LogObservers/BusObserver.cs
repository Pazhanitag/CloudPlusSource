using System;
using System.Threading.Tasks;
using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.LogObservers
{
    public class BusObserver : IBusObserver
    {
        public Task PostCreate(IBus bus)
        {
            // called after the bus has been created, but before it has been started.
            return Task.FromResult(0);
        }

        public Task CreateFaulted(Exception exception)
        {
            // called if the bus creation fails for some reason
            return Task.FromResult(0);
        }

        public Task PreStart(IBus bus)
        {
            // called just before the bus is started
            return Task.FromResult(0);
        }

        public Task StartFaulted(IBus bus, Exception exception)
        {
            // called if the bus fails to start for some reason (dead battery, no fuel, etc.)
            return Task.FromResult(0);
        }

        public Task PreStop(IBus bus)
        {
            // called just before the bus is stopped
            return Task.FromResult(0);
        }

        public Task PostStop(IBus bus)
        {
            // called after the bus has been stopped
            return Task.FromResult(0);
        }

        public Task StopFaulted(IBus bus, Exception exception)
        {
            // called if the bus fails to stop (no brakes)
            return Task.FromResult(0);
        }

        public Task PostStart(IBus bus, Task<BusReady> busReady)
        {
            // called once the bus has been started successfully. The task can be used to wait for
            // all of the receive endpoints to be ready.
            return Task.FromResult(0);
        }
    }
}