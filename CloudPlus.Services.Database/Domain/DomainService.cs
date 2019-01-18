using System.Collections.Generic;
using CloudPlus.Database;
using System.Linq;
using CloudPlus.Models.Domain;

namespace CloudPlus.Services.Database.Domain
{
    public class DomainService: IDomainService
    {
        private readonly CldpDbContext _dbContext;

        public DomainService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DomainResponseDto GetDomainByName(string name)
        {
            var domain = _dbContext.Domains.FirstOrDefault(d => d.Name == name);
            return domain != null ? new DomainResponseDto 
            {
                Id = domain.Id,
                Name = domain.Name,
                IsPrimary = domain.IsPrimary,
                CompanyId = domain.CompanyId
            } : null;
        }

        public bool IsDomainAvailable(string name)
        {
            var domain = _dbContext.Domains.FirstOrDefault(d => d.Name == name);
            return domain == null;
        }

        public IEnumerable<DomainResponseDto> GetCompanyDomains(int companyId)
        {
            return _dbContext.Domains.Where(d => d.CompanyId == companyId).Select(d => new DomainResponseDto
            {
                IsPrimary = d.IsPrimary,
                Name = d.Name
            });
        }
    }
}
