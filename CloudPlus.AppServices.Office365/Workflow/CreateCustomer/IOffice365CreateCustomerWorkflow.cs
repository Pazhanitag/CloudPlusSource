using CloudPlus.QueueModels.Office365.Customer.Commands;
using CloudPlus.Workflows.Common.Workflow;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Workflow.CreateCustomer
{
    public interface IOffice365CreateCustomerWorkflow : IWorkflow<ConsumeContext<IOffice365CreateCustommerCommand>>
    {
    }
}
