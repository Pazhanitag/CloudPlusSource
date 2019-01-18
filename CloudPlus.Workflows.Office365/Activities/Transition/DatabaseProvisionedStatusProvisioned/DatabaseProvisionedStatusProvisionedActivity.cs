using System;
using System.Threading.Tasks;
using CloudPlus.Enums.Provisions;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Provisions;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Office365.Activities.Transition.DatabaseProvisionedStatusProvisioned
{
    public class DatabaseProvisionedStatusProvisionedActivity : IDatabaseProvisionedStatusProvisionedActivity
    {
        private readonly IProvisioningService _provisioningService;

        public DatabaseProvisionedStatusProvisionedActivity(IProvisioningService provisioningService)
        {
            _provisioningService = provisioningService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IDatabaseProvisionedStatusProvisionedArguments> context)
        {
            var arguments = context.Arguments;

            await _provisioningService.UpdateStatusAsync(arguments.CompanyId, arguments.ProductId, CompanyProvisioningStatus.Provisioned);

            return context.Completed(new DatabaseProvisionedStatusProvisionedLog
            {
                CompanyId = arguments.CompanyId,
                ProductId = arguments.ProductId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IDatabaseProvisionedStatusProvisionedLog> context)
        {
            try
            {
                var logs = context.Log;

                await _provisioningService.UpdateStatusAsync(logs.CompanyId, logs.ProductId, CompanyProvisioningStatus.InTransition);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Componsate UpdateDatabaseProvisionedStatusActivity failed!", ex);
            }
            return context.Compensated();
        }
    }
}
