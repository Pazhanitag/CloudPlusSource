using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Services.Database.Office365.Offer
{
    public interface IOffice356DbOfferService
    {
        Task<List<Office365OfferModel>> GetOffice365OffersAsync();
        Task<Office365OfferModel> GetOffice365OfferAsync(string cloudPlusProductIdentifier);
    }
}
