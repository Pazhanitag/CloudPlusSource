using System.Collections.Generic;
using CloudPlus.Models.Domain;

namespace CloudPlus.Services.Database.Domain
{
    public interface IDomainService
    {
        DomainResponseDto GetDomainByName(string name);
        bool IsDomainAvailable(string name);
        IEnumerable<DomainResponseDto> GetCompanyDomains(int companyId);
    }
}
