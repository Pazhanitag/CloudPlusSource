using System;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.AppServices.Company.Workflow.CreateCompany;
using CloudPlus.QueueModels.Companies.Commands;
using MassTransit;
using CloudPlus.Services.Database.Domain;
using CloudPlus.Services.Database.WorkflowActivity.Company;

namespace CloudPlus.AppServices.Company.Consumers
{
    public class CreateCompanyConsumer : ICreateCompanyConsumer
    {

        private readonly ICreateCompanyWorkflow _createCompanyWorkflowBuilder;
        private readonly IWorkflowCompanyActivityService _workflowCompanyActivityService;
        private readonly IDomainService _domainService;

        public CreateCompanyConsumer(
            ICreateCompanyWorkflow createCompanyWorkflowBuilder,
            IWorkflowCompanyActivityService workflowCompanyActivityService,
            IDomainService domainService)
        {
            _createCompanyWorkflowBuilder = createCompanyWorkflowBuilder;
            _workflowCompanyActivityService = workflowCompanyActivityService;
            _domainService = domainService;
        }

        public async Task Consume(ConsumeContext<ICreateCompanyCommand> context)
        {
            if (!context.Message.Company.Domains.Any())
                throw new ArgumentException(nameof(context.Message.Company.Domains));

            if (context.Message.Company.Domains.Select(domain => _domainService.IsDomainAvailable(domain.Name)).Any(isDomainAvailable => !isDomainAvailable))
            {
                throw new Exception("Domain already taken!");
            }

            //TODO: Accept IEnumerable<string>
            if (_workflowCompanyActivityService.IsDomainBeingCreated(context.Message.Company.Domains.ToList()))
                throw new Exception("One of the domains is being used already!");

            await _createCompanyWorkflowBuilder.Execute(context);
        }
    }
}
