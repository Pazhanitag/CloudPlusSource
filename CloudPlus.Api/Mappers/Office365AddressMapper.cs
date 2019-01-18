using CloudPlus.Api.ViewModels.Request.Office365;
using CloudPlus.QueueModels.Office365.AddressValidation;

namespace CloudPlus.Api.Mappers
{
    public static class Office365AddressMapper
    {
        public static IOffice365AddresValidationRequest ToOffice365AddresValidationRequest(this Office365CustomerAddressViewModel viewModel)
        {
            return new Office365AddresValidationRequest
            {
                State = viewModel.State,
                City = viewModel.City,
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                Country = viewModel.Country,
                PostalCode = viewModel.PostalCode,
                PhoneNumber = viewModel.PhoneNumber,
            };
        }
    }
}