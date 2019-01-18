using System.Collections.Generic;
using System.Linq;
using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Models.Office365.License;

namespace CloudPlus.Api.Mappers
{
    public static class Office365LicenseMapper
    {
        public static Office365LicenseViewModel ToLicenseViewModel(this Office365LicenseModel model)
        {
            if (model == null)
                return null;

            return new Office365LicenseViewModel
            {
                Office365Offer = model.Office365Offer.ToOffice365OfferViewModel()
            };
        }
    }
}
