using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Api.Mappers
{
    public static class Office365OfferMapper
    {
        public static Office365OfferViewModel ToOffice365OfferViewModel(this Office365OfferModel model)
        {
            if (model == null)
                return null;

            return new Office365OfferViewModel
            {
                OfferName = model.OfferName,
                CloudPlusProductIdentifier = model.CloudPlusProductIdentifier
            };
        }
    }
}
