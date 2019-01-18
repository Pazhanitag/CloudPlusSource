using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Logging;
using CloudPlus.Services.Database.Company;

namespace CloudPlus.Workflows.Company.Activities.CreateDatabaseCompany
{
    public class CreateDatabaseCompanyActivity : ICreateDatabaseCompanyActivity
    {
        private readonly ICompanyService _companyService;

        public CreateDatabaseCompanyActivity(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateDatabaseCompanyArguments> context)
        {
            try
            {
                var company = context.Arguments.Company;

                company.CompanyOu = context.Arguments.CompanyOu;

                var dbCompany = await _companyService.CreateCompany(company);

                return context.CompletedWithVariables(new CreateDatabaseCompanyLog
                {
                    CompanyId = dbCompany.Id
                }, new
                {
                    CompanyId = dbCompany.Id
                });
            }
            catch (Exception ex)
            {
                this.Log().Error("Error executing CreateDatabaseCompanyActivity", ex);

                throw;
            }
        }

        public Task<CompensationResult> Compensate(CompensateContext<ICreateDatabaseCompanyLog> context)
        {
            try
            {
                _companyService.DeleteCompany(context.Log.CompanyId);

            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for CreateDatabaseCompanyActivity.", ex);
            }
            return Task.FromResult(context.Compensated());
        }
    }
}
