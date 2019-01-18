using System.Threading.Tasks;
using CloudPlus.AppServices.Office365.Workflow.CreateCustomer;
using CloudPlus.QueueModels.Office365.Customer.Commands;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.Customer
{
    public class Office365CreateCustomerConsumer : IOffice365CreateCustomerConsumer
    {
        private readonly IOffice365CreateCustomerWorkflow _createOffice365CustomerWorkflowBuilder;

        public Office365CreateCustomerConsumer(IOffice365CreateCustomerWorkflow createOffice365CustomerWorkflowBuilder)
        {
            _createOffice365CustomerWorkflowBuilder = createOffice365CustomerWorkflowBuilder;
        }

        public async Task Consume(ConsumeContext<IOffice365CreateCustommerCommand> context)
        {
            await _createOffice365CustomerWorkflowBuilder.Execute(context);
        }
    }
}