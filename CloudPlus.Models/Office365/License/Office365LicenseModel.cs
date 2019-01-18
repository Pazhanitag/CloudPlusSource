using CloudPlus.Models.Office365.Offer;

namespace CloudPlus.Models.Office365.License
{
    public class Office365LicenseModel
    {
        public int Id { get; set; }
        public Office365OfferModel Office365Offer { get; set; }
    }
}
