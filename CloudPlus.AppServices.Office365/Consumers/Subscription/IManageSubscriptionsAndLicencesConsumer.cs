using CloudPlus.QueueModels.Office365.Subscriptions.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Subscription
{
    public interface IManageSubscriptionsAndLicencesConsumer : IConsumer<IManageSubscriptionsAndLicencesCommand>
    {

    }
}