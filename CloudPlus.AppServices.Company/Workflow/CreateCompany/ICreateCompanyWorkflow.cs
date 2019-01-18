using CloudPlus.QueueModels.Companies.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Company.Workflow.CreateCompany
{
    public interface ICreateCompanyWorkflow : IWorkflow<ConsumeContext<ICreateCompanyCommand>>
    {
    }
}
