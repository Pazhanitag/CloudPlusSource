using System;
using System.Threading.Tasks;
using MassTransit;
using CloudPlus.AppServices.Office365.Workflow.ResendTxtRecords;
using CloudPlus.QueueModels.Office365.Domain.Commands;
using CloudPlus.Services.Database.Office365.Customer;

namespace CloudPlus.AppServices.Office365.Consumers.Domain
{
    public class Office365ResendTxtRecordsConsumer : IOffice365ResendTxtRecordsConsumer
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;
        private readonly IOffice365ResendTxtRecordsWorkflow _office365ResendTxtRecordsWorkflowBuilder;

        public Office365ResendTxtRecordsConsumer(
            IOffice365DbCustomerService office365DbCustomerService, 
            IOffice365ResendTxtRecordsWorkflow office365ResendTxtRecordsWorkflowBuilder)
        {
            _office365DbCustomerService = office365DbCustomerService;
            _office365ResendTxtRecordsWorkflowBuilder = office365ResendTxtRecordsWorkflowBuilder;
        }

        public async Task Consume(ConsumeContext<IOffice365ResendTxtRecordsCommand> context)
        {
            var message = context.Message;
            var office365Customer = await _office365DbCustomerService.GetOffice365CustomerWithIncludesAsync(message.CompanyId);

            if (office365Customer == null)
                throw new NullReferenceException(nameof(office365Customer));
            // [Kljuco 12/12/2017]: IDK if this is the best way. It feels funny. If someone has better solution PLEASE implement it.
            message.Office365CustomerId = office365Customer.Office365CustomerId;

            await _office365ResendTxtRecordsWorkflowBuilder.Execute(context);
        }
    }
}
