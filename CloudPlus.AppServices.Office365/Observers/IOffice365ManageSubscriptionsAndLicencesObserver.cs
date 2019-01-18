using MassTransit;
using CloudPlus.QueueModels.Office365.Subscriptions.Commands;

namespace CloudPlus.AppServices.Office365.Observers
{
    public interface IOffice365ManageSubscriptionsAndLicencesObserver : IConsumeMessageObserver<IManageSubscriptionsAndLicencesCommand>
    {
    }
}
