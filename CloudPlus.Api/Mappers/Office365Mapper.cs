using CloudPlus.Api.ViewModels.Request.Transition;

namespace CloudPlus.Api.Mappers
{
    public static class Office365Mapper
    {
        public static dynamic ToOffice365UserRemoveLicenseCommand(this Office365TransitionViewModel viewModel)
        {
            if (viewModel == null) return null;

            return new
            {
                viewModel.CompanyId,
                viewModel.ProductId,
                viewModel.Office365CustomerId,
                viewModel.Domains,
                viewModel.ProductItems
            };
        }
    }
}