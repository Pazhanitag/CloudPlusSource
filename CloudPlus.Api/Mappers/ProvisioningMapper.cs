using CloudPlus.Api.ViewModels.Response.User;
using CloudPlus.Models.Provisions;

namespace CloudPlus.Api.Mappers
{
    public static class ProvisioningMapper
    {
        public static UserServiceViewModel ToUserServiceViewModel(this ProvisionedServiceModel provisionedService)
        {
            if (provisionedService == null)
                return null;

            return new UserServiceViewModel
            {
                ProductId = provisionedService.ProductId,
                ImgUrl = provisionedService.ImgUrl,
                Name = provisionedService.Name,
                Status = provisionedService.Status,
                Vendor = provisionedService.Vendor,
                AssignedLicense = provisionedService.AssignedLicense,
                 AssignedLicenses=provisionedService.AssignedLicenses
            };
        }
    }
}