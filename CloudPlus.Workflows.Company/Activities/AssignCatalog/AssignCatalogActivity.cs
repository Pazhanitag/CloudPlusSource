using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Catalog;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.AssignCatalog
{
    public class AssignCatalogActivity : IAssignCatalogActivity
    {
        private readonly ICompanyCatalogService _companyCatalogService;

        public AssignCatalogActivity(ICompanyCatalogService companyCatalogService)
        {
            _companyCatalogService = companyCatalogService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IAssignCatalogArguments> context)
        {
            try
            {
                var arguments = context.Arguments;

                if(!arguments.ParentId.HasValue)
                    throw new Exception("Catalog cannot be assigned to master company");

                if (arguments.CatalogId.HasValue)
                {
                    await _companyCatalogService.AssignCatalogToCompany(arguments.ParentId.Value, arguments.CompanyId, arguments.CatalogId.Value);
                }
                else
                {
                    await _companyCatalogService.AssignDefaultCatalogToCompany(arguments.ParentId.Value, arguments.CompanyId);
                }
                
                await _companyCatalogService.GenerateDefaultCompanyCatalog(arguments.CompanyId);

                return context.Completed();
            }
            catch (Exception ex)
            {
                this.Log().Error("Error executing CreateCompanyPriceCatalogActivity", ex);
                throw;
            }
        }

        public Task<CompensationResult> Compensate(CompensateContext<IAssignCatalogLog> context)
        {
            return Task.FromResult(context.Compensated());
        }
    }
}
