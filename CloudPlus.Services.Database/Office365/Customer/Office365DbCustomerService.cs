using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Api;
using CloudPlus.Models.Office365.Customer;
using CloudPlus.Models.Office365.Domain;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Services.Database.Company;
using CloudPlus.Services.Database.WorkflowActivity.Office365;

namespace CloudPlus.Services.Database.Office365.Customer
{
    public class Office365DbCustomerService : IOffice365DbCustomerService
    {
        private readonly CldpDbContext _dbContext;
        private readonly ICompanyService _companyService;
        private readonly IWorkflowOffice365ActivityService _workflowOffice365ActivityService;

        public Office365DbCustomerService(
            CldpDbContext dbContext, 
            ICompanyService companyService,
            IWorkflowOffice365ActivityService workflowOffice365ActivityService)
        {
            _dbContext = dbContext;
            _companyService = companyService;
            _workflowOffice365ActivityService = workflowOffice365ActivityService;
        }

        public async Task<Office365CustomerModel> CreateDatabaseCustomerAsync(Office365CustomerModel customer)
        {
            var newCustomer = new Office365Customer
            {
                CompanyId = customer.CompanyId,
                Office365Id = customer.Office365CustomerId,
            };

            _dbContext.Office365Customers.Add(newCustomer);
            await _dbContext.SaveChangesAsync();

            customer.Id = newCustomer.Id;

            return customer;
        }

        public async Task DeleteDatabaseCustomerAsync(int id)
        {
            var office365Customer = _dbContext.Office365Customers.FirstOrDefault(c => c.Id == id);

            if (office365Customer == null)
                throw new NullReferenceException($"Could not delete Office 365 customer. There is no Office 365 customer with id {id}");

            _dbContext.Office365Domains.RemoveRange(office365Customer.Office365Domains);
            _dbContext.Office365Customers.Remove(office365Customer);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Office365CustomerModel> GetOffice365CustomerAsync(int companyId)
        {
            var office365Customer = await _dbContext.Office365Customers.FirstOrDefaultAsync(oc => oc.CompanyId == companyId);

            if (office365Customer == null)
                throw new NullReferenceException(nameof(office365Customer));

            return new Office365CustomerModel
            {
                Id = office365Customer.Id,
                Office365CustomerId = office365Customer.Office365Id,
                CompanyId = office365Customer.CompanyId
            };
        }

        public async Task<Office365CustomerModel> GetOffice365CustomerWithIncludesAsync(int companyId)
        {
            var office365Customer = await _dbContext.Office365Customers.AsNoTracking()
                .Include(d => d.Office365Domains)
                .Include(c => c.Office365Subscriptions.Select(o => o.Office365Offer))
                .FirstOrDefaultAsync(oc => oc.CompanyId == companyId);

            if (office365Customer == null)
                throw new NullReferenceException(nameof(office365Customer));

            return new Office365CustomerModel
            {
                Id = office365Customer.Id,
                Office365CustomerId = office365Customer.Office365Id,
                Domains = office365Customer.Office365Domains.Select(d => new Office365DomainModel
                {
                    Id = d.Id,
                    Name = d.DomainName,
                    IsFederated = d.IsFederated,
                    Office365DomainStatus = d.Office365DomainStaus
                }).ToList(),
                Office365Subscriptions = office365Customer.Office365Subscriptions.Select(s => new Office365SubscriptionModel
                {
                    Office365CustomerId = office365Customer.Office365Id,
                    Office365SubscriptionId = s.Office365SubscriptionId,
                    Quantity = s.Quantity,
                    Office365OrderId = s.Office365OrderId,
                    Office365Offer = new Office365OfferModel
                    {
                        CloudPlusProductIdentifier = s.Office365Offer.CloudPlusProductIdentifier
                    }
                }).ToList()
            };
        }

        public async Task<Office365CompanyModel> GetCompanyOffice365DomainsAsync(int companyId)
        {
            var dbCompany = await _companyService.GetCompanyAsync(companyId);

            var dbDomains = dbCompany.Domains.Select(d => new Office365DomainModel
            {
                Name = d.Name,
                Office365DomainStatus = Office365DomainStatus.NotConfigured
            }).ToList();

            var office365Customer = await GetOffice365CustomerWithIncludesAsync(companyId);
            var office365Domains = office365Customer.Domains;

            foreach (var domain in dbDomains)
            {
                var office365Domain = office365Domains.FirstOrDefault(d => d.Name == domain.Name);

                if (office365Domain == null) continue;

                domain.Office365DomainStatus = office365Domain.Office365DomainStatus;
                domain.IsFederated = office365Domain.IsFederated;
                domain.VerificationInProgress = _workflowOffice365ActivityService.IsOffice365DomainVerificationInProgress(domain.Name);
            }

            var company = new Office365CompanyModel()
            {
                CompanyId = dbCompany.Id,
                Email = dbCompany.Email,
                Domains = dbDomains.OrderByDescending(d => d.Office365DomainStatus).ToList(),
                Office365CustomerId = office365Customer.Office365CustomerId
            };

            return company;
        }
    }
}
