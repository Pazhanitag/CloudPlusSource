using MassTransit;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.AppServices.Office365.Observers
{
    public interface IOffice365TransitionDeletePartnerPlatformUserObserver : IConsumeMessageObserver<IOffice365TransitionDeletePartnerPlatformUserCommand>
    {
    }
}
