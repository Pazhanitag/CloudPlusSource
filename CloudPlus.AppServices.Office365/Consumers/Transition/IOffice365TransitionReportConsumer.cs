using MassTransit;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public interface IOffice365TransitionReportConsumer : IConsumer<IOffice365TransitionReportCommand>
    {
    }
}
