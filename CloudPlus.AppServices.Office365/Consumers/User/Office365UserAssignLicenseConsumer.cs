using System.Threading.Tasks;
using MassTransit;
using CloudPlus.AppServices.Office365.Workflow.UserAssignLicense;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserAssignLicenseConsumer : IOffice365UserAssignLicenseConsumer
    {
        private readonly IOffice365UserAssignLicenseWorkflow _office365UserAssignLicenseWorkflowBuilder;

        public Office365UserAssignLicenseConsumer(IOffice365UserAssignLicenseWorkflow office365UserAssignLicenseWorkflow)
        {
            _office365UserAssignLicenseWorkflowBuilder = office365UserAssignLicenseWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365UserAssignLicenseCommand> context)
        {
            await _office365UserAssignLicenseWorkflowBuilder.Execute(context);
        }
    }
}
