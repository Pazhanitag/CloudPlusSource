using System;
using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.UserRemoveLicense;
using MassTransit;
using CloudPlus.QueueModels.Office365.User.Commands;

namespace CloudPlus.AppServices.Office365.Consumers.User
{
    public class Office365UserRemoveLicenseConsumer : IOffice365UserRemoveLicenseConsumer
    {
        private readonly IOffice365UserRemoveLicenseWorkflow _office365UserRemoveLicenseWorkflow;

        public Office365UserRemoveLicenseConsumer(IOffice365UserRemoveLicenseWorkflow office365UserRemoveLicenseWorkflow)
        {
            _office365UserRemoveLicenseWorkflow = office365UserRemoveLicenseWorkflow;
        }

        public async Task Consume(ConsumeContext<IOffice365UserRemoveLicenseCommand> context)
        {
            await _office365UserRemoveLicenseWorkflow.Execute(context);
        }
    }
}
