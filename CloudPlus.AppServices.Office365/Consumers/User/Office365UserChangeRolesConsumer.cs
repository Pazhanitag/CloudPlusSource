using System.Threading.Tasks;
using MassTransit;
using CloudPlus.AppServices.Office365.Workflow.UserChangeRoles;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserChangeRolesConsumer : IOffice365UserChangeRolesConsumer
    {
        private readonly IOffice365UserChangeRolesWorkflow _office365UserChangeRolesWorkflow;

        public Office365UserChangeRolesConsumer(IOffice365UserChangeRolesWorkflow office365UserChangeRolesWorkflow)
        {
            _office365UserChangeRolesWorkflow = office365UserChangeRolesWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365UserChangeRolesCommand> context)
        {
            await _office365UserChangeRolesWorkflow.Execute(context);
        }
    }
}
