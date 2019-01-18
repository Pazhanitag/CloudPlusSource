using CloudPlus.Api.ViewModels.Request.Company;
using CloudPlus.Api.ViewModels.Response.Company;
using CloudPlus.Models.Domain;

namespace CloudPlus.Api.Mappers
{
    public static class DomainMapper
    {
        public static DomainViewModel ToDomainViewModel(this DomainModel domain)
        {
            if (domain == null)
                return null;

            return new DomainViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                IsPrimary = domain.IsPrimary
            };
        }

        public static DomainModel ToDomainModel(this CreateDomainViewModel domain)
        {
            if (domain == null)
                return null;

            return new DomainModel
            {
                Name = domain.Name,
                IsPrimary = domain.IsPrimary
            };
        }
    }
}