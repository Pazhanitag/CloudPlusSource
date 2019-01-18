using System.Threading.Tasks;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Services.Database.Office365.Subscription
{
    public interface IOffice365DbSubscriptionService
    {
        Task<Office365SubscriptionModel> GetSubscriptionAsyc(string office365SubscriptionId);
        Task<Office365SubscriptionModel> GetSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier);
        Task<Office365SubscriptionModel> CreateSubscription(Office365SubscriptionModel model);
        Task AddPartnerPlatformDataToSubscription(Office365SubscriptionModel model);
        Task IncreaseSubscription(string office365SubscriptionId);
        Task IncreaseSubscriptionByProductIdentifierAsync(string office365CustomerId,
            string cloudplusProductIdentifier);
        Task DecreaseSubscription(string office365SubscriptionId);
        Task DecreaseSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier);
        Task DeleteSubscription(string office365SubscriptionId);
        Task SuspendSubscription(string office365SubscriptionId);
        Task UnsuspendSubscription(string office365SubscriptionId);
        Task DeleteSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier);

        Task UpdateDatabseSubscriptionState(Office365SubscriptionState subscriptionState, string ffice365SubscriptionId);
        Task UpdateSubscriptionQuantity(string office365SubscriptionId, int quantityChange);
    }
}
