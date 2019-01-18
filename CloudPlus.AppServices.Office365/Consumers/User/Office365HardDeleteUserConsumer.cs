using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.HardDeleteUser;
using CloudPlus.QueueModels.Office365.User.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365HardDeleteUserConsumer : IOffice365HardDeleteUserConsumer
    {
        private readonly IHardDeleteUserWorkflow _hardDeleteUserWorkflow;

        public Office365HardDeleteUserConsumer(IHardDeleteUserWorkflow hardDeleteUserWorkflow)
        {
            _hardDeleteUserWorkflow = hardDeleteUserWorkflow;
        }
        public async Task Consume(ConsumeContext<IOffice365HardDeleteUserCommand> context)
        {
            await _hardDeleteUserWorkflow.Execute(context);
        }
    }
}