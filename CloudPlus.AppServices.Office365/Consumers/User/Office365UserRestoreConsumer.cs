using CloudPlus.AppServices.Office365.Workflow.UserRestore;
using CloudPlus.QueueModels.Office365.User.Commands;
using MassTransit;
using System.Threading.Tasks;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserRestoreConsumer : IOffice365UserRestoreConsumer
    {
        private readonly IOffice365UserRestoreWorkflow _office365UserRestoreWorkflow;

        public Office365UserRestoreConsumer(IOffice365UserRestoreWorkflow office365UserRestoreWorkflow)
        {
            _office365UserRestoreWorkflow = office365UserRestoreWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365UserRestoreCommand> context)
        {
            await _office365UserRestoreWorkflow.Execute(context);
        }
    }
}
