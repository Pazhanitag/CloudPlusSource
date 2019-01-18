namespace CloudPlus.Models.Office365.Order
{
    public class Office365OrderModel
    {
        public string Office365CustomerId { get; set; }
        public string Office365OfferId { get; set; }
        public string FriendlyName { get; set; }
        public int Quantity { get; set; }
        public string SubscriptionId { get; set; }
        //TODO Remove from model
        public string OrderId { get; set; }
    }
}
