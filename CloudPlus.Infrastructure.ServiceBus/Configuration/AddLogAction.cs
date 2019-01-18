using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.Configuration
{
    /// <summary>
    ///     http://masstransit-project.com/MassTransit/usage/observers.html
    /// </summary>
    public class AddLogAction : IAddLogAction
    {
        private readonly IBusControl _busControl;

        public AddLogAction(IBusControl busControl)
        {
            _busControl = busControl;
        }

        /// <summary>
        ///     Observe received messages immediately after they are delivered by the transport
        /// </summary>
        /// <param name="receiveObserver"></param>
        public void AddLog(IReceiveObserver receiveObserver)
        {
            _busControl.ConnectReceiveObserver(receiveObserver);
        }

        /// <summary>
        ///     Observe consumed messages
        /// </summary>
        /// <param name="consumeObserver"></param>
        public void AddLog(IConsumeObserver consumeObserver)
        {
            _busControl.ConnectConsumeObserver(consumeObserver);
        }

        /// <summary>
        ///     Observing published messages
        /// </summary>
        /// <param name="publishObserver"></param>
        public void AddLog(IPublishObserver publishObserver)
        {
            _busControl.ConnectPublishObserver(publishObserver);
        }

        /// <summary>
        ///     Observing sent messages
        /// </summary>
        /// <param name="sendObserver"></param>
        public void AddLog(ISendObserver sendObserver)
        {
            _busControl.ConnectSendObserver(sendObserver);
        }

        public void AddLog(IBusObserver busObserver)
        {
            //_rabbitMqBusFactoryConfigurator.c
        }

        /// <summary>
        ///     Observing specific consumed messages
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="consumeMessageObserver"></param>
        public void AddLog<T>(IConsumeMessageObserver<T> consumeMessageObserver) where T : class
        {
            _busControl.ConnectConsumeMessageObserver(consumeMessageObserver);
        }
    }
}