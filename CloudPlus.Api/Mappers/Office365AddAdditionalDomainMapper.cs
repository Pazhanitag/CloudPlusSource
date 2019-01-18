using CloudPlus.Api.ViewModels.Request.Office365;

namespace CloudPlus.Api.Mappers
{
    public static class Office365AddAdditionalDomainMapper
    {
        public static dynamic ToOffice365AddAdditionalDomainCommand(this Office365AddAdditionalDomainViewModel viewModel)
        {
            return new
            {
                viewModel.Office365CustomerId,
                viewModel.CompanyId,
                viewModel.Domain,
                viewModel.Email
            };
        }
    }
}
