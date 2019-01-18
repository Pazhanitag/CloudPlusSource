using CloudPlus.QueueModels.Office365.AddressValidation;
using MassTransit;

namespace CloudPlus.AppServices.Office365.Consumers.AddressValidation
{
    public interface IOffice365AddressValidationConsumer: IConsumer<IOffice365AddresValidationRequest>
    {
    }
}
