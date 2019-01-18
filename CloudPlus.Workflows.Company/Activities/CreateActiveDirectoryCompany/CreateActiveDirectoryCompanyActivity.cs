using System;
using System.Threading.Tasks;
using CloudPlus.Logging;
using CloudPlus.Services.ActiveDirectory;
using CloudPlus.Services.ActiveDirectory.Models;
using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.CreateActiveDirectoryCompany
{
    public class CreateActiveDirectoryComapnyActivity : ICreateActiveDirectoryComapnyActivity
    {
        private readonly IActiveDirectoryService _activeDirectoryService;

        public CreateActiveDirectoryComapnyActivity(IActiveDirectoryService activeDirectoryService)
        {
            _activeDirectoryService = activeDirectoryService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreateActiveDirectoryCompanyArguments> context)
        {
            try
            {
                var companyOu = await _activeDirectoryService.GenerateCompanyOuIdAsync();

                await _activeDirectoryService.CreateCompany(new ActiveDirectoryCompany
                {
                    CompanyOu = companyOu
                });

                return context.CompletedWithVariables(new CreateActiveDirectoryCompanyLog
                {
                    CompanyOu = companyOu
                }, new
                {
                    CompanyOu = companyOu
                });
            }
            catch (System.Exception ex)
            {
                this.Log().Error("Error executing CreateActiveDirectoryComapnyActivity", ex);
                throw;
            }
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ICreateActiveDirectoryCompanyLog> context)
        {
            try
            {
                await _activeDirectoryService.DeleteCompany(context.Log.CompanyOu);

            }
            catch (Exception ex)
            {
                this.Log().Fatal("Could not compensate for CreateActiveDirectoryComapnyActivity.", ex);
            }
            return await Task.FromResult(context.Compensated());
        }
    }
}
