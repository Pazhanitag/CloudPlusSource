using System.Threading.Tasks;
using CloudPlus.Models.Office365.Address;
using CloudPlus.QueueModels.Office365.AddressValidation;
using CloudPlus.Services.Office365.AddressService;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.AddressValidation
{
    public class Office365AddressValidationConsumer : IOffice365AddressValidationConsumer
    {
        private readonly IAddressValidationService _addressValidationService;

        public Office365AddressValidationConsumer(IAddressValidationService addressValidationService)
        {
            _addressValidationService = addressValidationService;
        }
        public async Task Consume(ConsumeContext<IOffice365AddresValidationRequest> context)
        {
            var isAddressValid = await _addressValidationService.IsAddressValidAsync(new AddressValidationModel
            {
                AddressLine1 = context.Message.AddressLine1,
                AddressLine2 = context.Message.AddressLine2,
                City = context.Message.City,
                State = context.Message.State,
                Country = context.Message.Country,
                PostalCode = context.Message.PostalCode,
                PhoneNumber = context.Message.PhoneNumber
            });

            await context.RespondAsync(new Office365AddresValidationResponse
            {
                IsAddressValid = isAddressValid
            });
        }
    }
}
