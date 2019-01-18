using System.Threading.Tasks;
using MassTransit;
using CloudPlus.AppServices.Office365.Workflow.Transition;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public class Office365TransitionConsumer : IOffice365TransitionConsumer
    {
        private readonly IOffice365TransitionWorkflow _office365TransitionWorkflow;

        public Office365TransitionConsumer(IOffice365TransitionWorkflow office365TransitionWorkflow)
        {
            _office365TransitionWorkflow = office365TransitionWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365TransitionCommand> context)
        {
            await _office365TransitionWorkflow.Execute(context);
        }
    }
}
