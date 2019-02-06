using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Order;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Services.Office365.Subscription
{
    public interface IOffice365SubscriptionService
    {
        Task<Office365SubscriptionModel> GetPartnerPlatformSubscriptionByOfferAsync(string office365CustomerId, string cloudPlusProductIdentifier);
        Task<Office365SubscriptionModel> CreatePartnerPlatformSubscriptionAsync(string office365CustomerId, string cloudPlusProductIdentifier, int quantity = 1);
        //TAG
        Task<Office365OrderSubscriptionModel> CreateMultiPartnerPlatformSubscriptionsAsync(Office365OrderWithDetailsModel office365OrderModel);
        Task<Office365SubscriptionModel> ActivateSuspendedPartnerPlatformSubscriptionAsync(string office365CustomerId, string office365SubscriptionId);
        //TAG
        Task<List<Office365SubscriptionModel>> ActivateMultiSuspendedPartnerPlatformSubscriptionAsync(string office365CustomerId, List<string> office365SubscriptionId);
        Task SuspendPartnerPlatformSubscriptionAsync(string office365CustomerId, string office365SubscriptionId);
        Task SuspendMultiPartnerPlatformSubscriptionAsync(string office365CustomerId, List<string> office365SubscriptionId);
        Task<Office365SubscriptionModel> IncreasePartnerPlatformSubscriptionQuantityAsync(string office365CustomerId, string office365SubscriptionId);
        Task<Office365SubscriptionModel> DecreasePartnerPlatformSubscriptionQuantityAsync(string office365CustomerId, string office365SubscriptionId);
        Task<Office365SubscriptionModel> UpdatePartnerPlatformSubscriptionQuantity(string office365CustomerId, string office365SubscriptionId, int quantityChange);
    }
}
