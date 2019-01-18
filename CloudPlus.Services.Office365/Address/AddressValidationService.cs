using System;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter.Models;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Address;
using CloudPlus.Services.Office365.Operations;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.AddressService
{
    public class AddressValidationService : IAddressValidationService
    {
        private readonly IPartnerOperations _partnerOperations;

        public AddressValidationService(IPartnerOperations partnerOperations)
        {
            _partnerOperations = partnerOperations;
        }

        public async Task<bool> IsAddressValidAsync(AddressValidationModel addressModel)
        {
            var isAddressValid = false;
            try
            {
                isAddressValid = await _partnerOperations.UserPartnerOperations.Validations.IsAddressValidAsync(
                    new Address
                    {
                        AddressLine1 = addressModel.AddressLine1,
                        AddressLine2 = addressModel.AddressLine2,
                        City = addressModel.City,
                        State = addressModel.State,
                        Country = addressModel.Country,
                        PostalCode = addressModel.PostalCode,
                        PhoneNumber = addressModel.PhoneNumber,
                    });
            }
            catch (Exception ex)
            {
                this.Log().Error("Error validating user address", ex);
            }
            return isAddressValid;
        }
    }
}
