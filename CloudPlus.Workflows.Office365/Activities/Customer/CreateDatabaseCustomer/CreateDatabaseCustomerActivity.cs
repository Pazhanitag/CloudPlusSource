using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Customer;
using CloudPlus.Services.Database.Office365.Customer;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreateDatabaseCustomer
{
    public class CreateDatabaseCustomerActivity : ICreateDatabaseCustomerActivity
    {
        private readonly IOffice365DbCustomerService _office365DbCustomerService;

        public CreateDatabaseCustomerActivity(IOffice365DbCustomerService office365DbCustomerService)
        {
            _office365DbCustomerService = office365DbCustomerService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseCustomerArguments> context)
        {
            var arguments = context.Arguments;

            var office365Customer = await _office365DbCustomerService.CreateDatabaseCustomerAsync(new Office365CustomerModel
            {
                CompanyId = arguments.CompanyId,
                Office365CustomerId = arguments.Office365CustomerId
            });

            return context.CompletedWithVariables(new CreateDatabaseCustomerLog
            {
                Id = office365Customer.Id
            }, new
            {
                Office365DatabaseCustomerId = office365Customer.Id
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateDatabaseCustomerLog> context)
        {
            var log = context.Log;
            try
            {
                await _office365DbCustomerService.DeleteDatabaseCustomerAsync(log.Id);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for CreateDatabaseCustomerActivity.", ex);
            }
            return context.Compensated();
        }
    }
}
