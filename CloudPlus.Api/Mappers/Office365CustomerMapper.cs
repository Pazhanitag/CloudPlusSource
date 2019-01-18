using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.QueueModels.Office365.AddressValidation;
using CloudPlus.QueueModels.Office365.Customer.Commands;

namespace CloudPlus.Api.Mappers
{
    public static class Office365CustomerMapper
    {
        public static dynamic ToOffice365CreateCustomerCommand(this Office365CreateCustomerViewModel viewModel)
        {
            return new
            {
                viewModel.City,
                viewModel.AddressLine1,
                viewModel.AddressLine2,
                viewModel.CompanyName,
                viewModel.Country,
                viewModel.Culture,
                viewModel.Domain,
                viewModel.Email,
                viewModel.FirstName,
                viewModel.Language,
                viewModel.LastName,
                viewModel.PhoneNumber,
                viewModel.State,
                viewModel.PostalCode,
                viewModel.CompanyId
            };
        }
    }
}