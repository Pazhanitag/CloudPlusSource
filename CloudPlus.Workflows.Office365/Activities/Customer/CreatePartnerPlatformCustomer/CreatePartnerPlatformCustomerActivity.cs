using System;
using System.Threading.Tasks;
using MassTransit.Courier;
using CloudPlus.Services.Database.Company;
using CloudPlus.Models.Office365.Customer;
using CloudPlus.Services.Office365.CustomerService;

namespace CloudPlus.Workflows.Office365.Activities.Customer.CreatePartnerPlatformCustomer
{
    public class CreatePartnerPlatformCustomerActivity : ICreatePartnerPlatformCustomerActivity
    {
        private readonly IOffice365CustomerService _office365CustomerService;
        private readonly ICompanyService _companyService;

        public CreatePartnerPlatformCustomerActivity(IOffice365CustomerService office365CustomerService, ICompanyService companyService)
        {
            _office365CustomerService = office365CustomerService;
            _companyService = companyService;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ICreatePartnerPlatformCustomerArguments> context)
        {
            var arguments = context.Arguments;

            var company = await _companyService.GetCompanyAsync(arguments.CompanyId);

            if (company == null)
                throw new NullReferenceException($"Could not find company with Id {arguments.CompanyId}");


            var createdCustomer = await _office365CustomerService.CreatePartnerPlatformCustomerAsync(new Office365PartnerPlatformCustomerModel
            {
                AddressLine1 = arguments.AddressLine1,
                AddressLine2 = arguments.AddressLine2,
                City = arguments.City,
                CompanyName = arguments.CompanyName,
                Country = arguments.Country,
                Culture = arguments.Culture,
                Email = arguments.Email,
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                Language = arguments.Language,
                PhoneNumber = arguments.PhoneNumber,
                PostalCode = arguments.PostalCode,
                State = arguments.State,
                CompanyOu = company.CompanyOu
            });

            return context.CompletedWithVariables(new CreatePartnerPlatformCustomerLog
            {
                Office365CustomerId = createdCustomer.Id
            }, new
            {
                Office365CustomerId = createdCustomer.Id
            });
        }

        public Task<CompensationResult> Compensate(CompensateContext<ICreatePartnerPlatformCustomerLog> context)
        {
            // Not sure what to do here!?

            return Task.FromResult(context.Compensated());
        }
    }
}
