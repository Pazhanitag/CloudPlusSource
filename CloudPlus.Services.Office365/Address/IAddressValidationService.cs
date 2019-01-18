using System.Threading.Tasks;
using CloudPlus.Models.Office365.Address;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.AddressService
{
    public interface IAddressValidationService
    {
        Task<bool> IsAddressValidAsync(AddressValidationModel addressModel);
    }
}
