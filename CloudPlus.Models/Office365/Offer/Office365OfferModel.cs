namespace CloudPlus.Models.Office365.Offer
{
    public class Office365OfferModel
    {
        public int Id { get; set; }
        public string Office365Id { get; set; }
        public string OfferName { get; set; }
        public string Sku { get; set; } // ?????
        public string CloudPlusProductIdentifier { get; set; }
        public bool IsAddon { get; set; }
    }
}
