using MassTransit;

namespace CloudPlus.Infrastructure.ServiceBus.Configuration
{
    public interface IAddLogAction
    {
        void AddLog(IReceiveObserver receiveObserver);
        void AddLog(IConsumeObserver consumeObserver);
        void AddLog(IPublishObserver publishObserver);
        void AddLog(ISendObserver sendObserver);
        void AddLog(IBusObserver busObserver);
        void AddLog<T>(IConsumeMessageObserver<T> consumeMessageObserver) where T : class;
    }
}