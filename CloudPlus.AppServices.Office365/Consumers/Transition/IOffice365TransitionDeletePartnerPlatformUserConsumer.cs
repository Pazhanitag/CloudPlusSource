using CloudPlus.QueueModels.Office365.Transition.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public interface IOffice365TransitionDeletePartnerPlatformUserConsumer 
        : IConsumer<IOffice365TransitionDeletePartnerPlatformUserCommand>
    {
    }
}
