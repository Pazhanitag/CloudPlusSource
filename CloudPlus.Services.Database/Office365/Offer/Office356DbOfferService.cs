using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Services.Database.Office365.Offer
{
    public class Office356DbOfferService : IOffice356DbOfferService
    {
        private readonly CldpDbContext _dbContext;

        public Office356DbOfferService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Office365OfferModel>> GetOffice365OffersAsync()
        {
            var offers = await _dbContext.Office365Offers.ToListAsync();

            return new List<Office365OfferModel>(offers.Select(offer => new Office365OfferModel
            {
                Id = offer.Id,
                Office365Id = offer.Office365OfferId,
                OfferName = offer.Office365OfferName,
                Sku = offer.Office365ProductSku,
                CloudPlusProductIdentifier = offer.CloudPlusProductIdentifier
            }));
        }

        public async Task<Office365OfferModel> GetOffice365OfferAsync(string cloudPlusProductIdentifier)
        {
            var offer = await _dbContext.Office365Offers.FirstOrDefaultAsync(o => o.CloudPlusProductIdentifier == cloudPlusProductIdentifier);

            if (offer == null)
                throw new NullReferenceException(nameof(offer));

            return new Office365OfferModel
            {
                Id = offer.Id,
                Office365Id = offer.Office365OfferId,
                OfferName = offer.Office365OfferName,
                Sku = offer.Office365ProductSku,
                CloudPlusProductIdentifier = offer.CloudPlusProductIdentifier
            };
        }
    }
}
