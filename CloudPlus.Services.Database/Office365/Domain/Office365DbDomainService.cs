using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Api;

namespace CloudPlus.Services.Database.Office365.Domain
{
    public class Office365DbDomainService : IOffice365DbDomainService
    {
        private readonly CldpDbContext _dbContext;
        public Office365DbDomainService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Office365CustomerDomainModel> CreateDatabaseCustomerDomainAsync(Office365CustomerDomainModel domain)
        {
            var customer = _dbContext.Office365Customers.FirstOrDefault(c => c.Office365Id == domain.Office365CustomerId);

            if (customer == null)
                throw new Exception($"No customer with Office 365 Customer Id: {domain.Office365CustomerId}");

            var newDomain = new Office365Domain
            {
                DomainName = domain.Domain.ToLower(),
                Office365DomainStaus = domain.Office365DomainStaus,
                Office365Customer = customer
            };

            _dbContext.Office365Domains.Add(newDomain);
            await _dbContext.SaveChangesAsync();

            return domain;
        }

        public async Task DeleteDomainAsync(string domain)
        {
            var domainDb = await _dbContext.Office365Domains
                .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.ToLower());

            if (domainDb == null)
                throw new Exception($"No Office 365 domain: {domain}");

            _dbContext.Office365Domains.Remove(domainDb);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeDatabaseCustomerDomainVerifyStatusAsync(string domain, Office365DomainStatus status)
        {
            var office365Domain = await _dbContext.Office365Domains
                .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.ToLower());

            if (office365Domain == null) throw new ArgumentException($"Could not find Office 365 domain: {domain}");

            office365Domain.Office365DomainStaus = status;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeDatabaseCustomerDomainFederatedStatusAsync(string domain, bool status)
        {
            var office365Domain = await _dbContext.Office365Domains
                .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.ToLower());
            
            if (office365Domain == null) throw new ArgumentException($"Could not find Office 365 domain: {domain}");

            office365Domain.IsFederated = status;

            await _dbContext.SaveChangesAsync();
        }

        public bool IsAnyDomainAdded(int companyId)
        {
            var office365Customer = _dbContext.Office365Customers.FirstOrDefault(c => c.CompanyId == companyId);

            if (office365Customer != null)
            {
                var office365Domain = _dbContext.Office365Domains.FirstOrDefault(d => d.Office365Customer.Id == office365Customer.Id);
                if (office365Domain != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsDomainVerified(string domain)
        {
            var office365Domain = await _dbContext.Office365Domains
                .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.ToLower());
            
            return office365Domain != null && office365Domain.Office365DomainStaus == Office365DomainStatus.Validated;
        }

        public async Task<bool> IsDomainFederated(string domain)
        {
            var office365Domain = await _dbContext.Office365Domains
                .FirstOrDefaultAsync(d => d.DomainName.ToLower() == domain.ToLower());
            return office365Domain != null && office365Domain.IsFederated;
        }
    }
}
