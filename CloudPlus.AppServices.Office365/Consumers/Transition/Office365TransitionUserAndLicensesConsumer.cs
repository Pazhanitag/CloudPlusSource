using System.Threading.Tasks;
using MassTransit;
using CloudPlus.AppServices.Office365.Workflow.Transition;
using CloudPlus.QueueModels.Office365.Transition.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.Transition
{
    public class Office365TransitionUserAndLicensesConsumer : IOffice365TransitionUserAndLicensesConsumer
    {
        private readonly IOffice365TransitionUserAndLicensesWorkflow _workflow;

        public Office365TransitionUserAndLicensesConsumer(IOffice365TransitionUserAndLicensesWorkflow workflow)
        {
            _workflow = workflow;
        }

        public async Task Consume(ConsumeContext<IOffice365TransitionUserAndLicensesCommand> context)
        {
            await _workflow.Execute(context);
        }
    }
}
