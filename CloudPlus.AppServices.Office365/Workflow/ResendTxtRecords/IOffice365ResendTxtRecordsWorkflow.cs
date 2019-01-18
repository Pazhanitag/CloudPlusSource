using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.ResendTxtRecords
{
    public interface IOffice365ResendTxtRecordsWorkflow : IWorkflow<ConsumeContext<IOffice365ResendTxtRecordsCommand>>
    {
    }
}
