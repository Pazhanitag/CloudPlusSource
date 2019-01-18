using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Models.Office365.Domain;

namespace CloudPlus.Api.Mappers
{
    public static class Office365DomainMapper
    {
        public static Office365DomainViewModel ToOffice365DomainViewModel(this Office365DomainModel domain)
        {
            if (domain == null)
                return null;

            return new Office365DomainViewModel
            {
                Name = domain.Name,
                Status = domain.Office365DomainStatus,
                IsFederated = domain.IsFederated,
                VerificationInProgress = domain.VerificationInProgress,
            };
        }
    }
}