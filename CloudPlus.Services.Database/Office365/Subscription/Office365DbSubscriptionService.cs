using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Database;
using CloudPlus.Entities.Office365;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.Offer;
using CloudPlus.Models.Office365.Subscription;

namespace CloudPlus.Services.Database.Office365.Subscription
{
    public class Office365DbSubscriptionService : IOffice365DbSubscriptionService
    {
        private readonly CldpDbContext _dbContext;

        public Office365DbSubscriptionService(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Office365SubscriptionModel> GetSubscriptionAsyc(string office365SubscriptionId)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .FirstOrDefaultAsync(s => s.Office365SubscriptionId == office365SubscriptionId);

            if (subscription == null) return null;

            return new Office365SubscriptionModel
            {
                Office365CustomerId = subscription.Office365Customer.Office365Id,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Office365OrderId = subscription.Office365OrderId,
                Office365FriendlyName = subscription.Office365FriendlyName,
                Quantity = subscription.Quantity,
                Office365Offer = new Office365OfferModel
                {
                    Id = subscription.Office365Offer.Id,
                    Office365Id = subscription.Office365Offer.Office365OfferId,
                    OfferName = subscription.Office365Offer.Office365OfferName,
                    Sku = subscription.Office365Offer.Office365ProductSku,
                    CloudPlusProductIdentifier = subscription.Office365Offer.CloudPlusProductIdentifier
                }
            };
        }

        //TAG
        public async Task<List<Office365SubscriptionModel>> GetSubscriptionListAsyc(List<string> office365SubscriptionId)
        {
            var subscriptions = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .Where(s => office365SubscriptionId.Any(o => o.Contains(s.Office365SubscriptionId))).ToListAsync();

            if (subscriptions == null || subscriptions.Count == 0) return null;

            return subscriptions.Select(x => new Office365SubscriptionModel
            {
                Office365CustomerId = x.Office365Customer.Office365Id,
                Office365SubscriptionId = x.Office365SubscriptionId,
                Office365OrderId = x.Office365OrderId,
                Office365FriendlyName = x.Office365FriendlyName,
                Quantity = x.Quantity,
                Office365Offer = new Office365OfferModel
                {
                    Id = x.Office365Offer.Id,
                    Office365Id = x.Office365Offer.Office365OfferId,
                    OfferName = x.Office365Offer.Office365OfferName,
                    Sku = x.Office365Offer.Office365ProductSku,
                    CloudPlusProductIdentifier = x.Office365Offer.CloudPlusProductIdentifier
                }
            }).ToList();
        }

        public async Task<Office365SubscriptionModel> GetSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Office365Customer.Office365Id == office365CustomerId &&
                                          s.Office365Offer.CloudPlusProductIdentifier == cloudplusProductIdentifier);
           
            if (subscription == null) return null;

            return new Office365SubscriptionModel
            {
                Office365CustomerId = subscription.Office365Customer.Office365Id,
                Office365SubscriptionId = subscription.Office365SubscriptionId,
                Office365OrderId = subscription.Office365OrderId,
                Office365FriendlyName = subscription.Office365FriendlyName,
                Quantity = subscription.Quantity,
                SubscriptionState = subscription.SubscriptionState,
                Office365Offer = new Office365OfferModel
                {
                    Id = subscription.Office365Offer.Id,
                    Office365Id = subscription.Office365Offer.Office365OfferId,
                    OfferName = subscription.Office365Offer.Office365OfferName,
                    Sku = subscription.Office365Offer.Office365ProductSku,
                    CloudPlusProductIdentifier = subscription.Office365Offer.CloudPlusProductIdentifier
                },
                Suspended = subscription.Suspended
            };
        }

        //TAG
        public async Task<List<Office365SubscriptionModel>> GetSubscriptionsByProductIdentifiersAsync(
            string office365CustomerId, IEnumerable<string> cloudplusProductIdentifiers)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer).AsNoTracking()
                .Where(s => s.Office365Customer.Office365Id == office365CustomerId &&
                                           cloudplusProductIdentifiers.Contains(s.Office365Offer.CloudPlusProductIdentifier)).ToListAsync();

            if (subscription == null) return null;

            return subscription.Select(s => new Office365SubscriptionModel
            {
                Office365CustomerId = s.Office365Customer.Office365Id,
                Office365SubscriptionId = s.Office365SubscriptionId,
                Office365OrderId = s.Office365OrderId,
                Office365FriendlyName = s.Office365FriendlyName,
                Quantity = s.Quantity,
                SubscriptionState = s.SubscriptionState,
                Office365Offer = new Office365OfferModel
                {
                    Id = s.Office365Offer.Id,
                    Office365Id = s.Office365Offer.Office365OfferId,
                    OfferName = s.Office365Offer.Office365OfferName,
                    Sku = s.Office365Offer.Office365ProductSku,
                    CloudPlusProductIdentifier = s.Office365Offer.CloudPlusProductIdentifier
                },
                Suspended = s.Suspended
            }).ToList();
        }

        public async Task<Office365SubscriptionModel> CreateSubscription(Office365SubscriptionModel model)
        {
            _dbContext.Office365Subscriptions.Add(new Office365Subscription
            {
                Office365SubscriptionId = model.Office365SubscriptionId,
                Office365OrderId = model.Office365OrderId,
                Office365FriendlyName = model.Office365FriendlyName,
                Quantity = model.Quantity,
                SubscriptionState = model.SubscriptionState,
                Office365Customer = _dbContext.Office365Customers.FirstOrDefault(c => c.Office365Id == model.Office365CustomerId),
                Office365Offer = _dbContext.Office365Offers
                    .FirstOrDefault(o => o.CloudPlusProductIdentifier == model.Office365Offer.CloudPlusProductIdentifier),
            });

            await _dbContext.SaveChangesAsync();

            return model;
        }

        public async Task AddPartnerPlatformDataToSubscription(Office365SubscriptionModel model)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .FirstOrDefaultAsync(s => s.Office365Customer.Office365Id == model.Office365CustomerId &&
                                          s.Office365Offer.CloudPlusProductIdentifier == model.Office365Offer.CloudPlusProductIdentifier);

            subscription.Office365SubscriptionId = model.Office365SubscriptionId;
            subscription.Office365OrderId = model.Office365OrderId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task IncreaseSubscription(string office365SubscriptionId)
        {
            var subscription = await GetSubscriptionEntity(office365SubscriptionId);

            subscription.Quantity++;

            await _dbContext.SaveChangesAsync();
        }

        public async Task IncreaseSubscriptionByProductIdentifierAsync(string office365CustomerId, string cloudplusProductIdentifier)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .FirstOrDefaultAsync(s => s.Office365Customer.Office365Id == office365CustomerId &&
                                          s.Office365Offer.CloudPlusProductIdentifier == cloudplusProductIdentifier);

            subscription.Quantity++;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DecreaseSubscription(string office365SubscriptionId)
        {
            var subscription = await GetSubscriptionEntity(office365SubscriptionId);

            subscription.Quantity--;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DecreaseSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .FirstOrDefaultAsync(s => s.Office365Customer.Office365Id == office365CustomerId &&
                                          s.Office365Offer.CloudPlusProductIdentifier == cloudplusProductIdentifier);

            subscription.Quantity--;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSubscription(string office365SubscriptionId)
        {
            var subscription = await GetSubscriptionEntity(office365SubscriptionId);

            _dbContext.Office365Subscriptions.Remove(subscription);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SuspendSubscription(string office365SubscriptionId)
        {
            var subscription =
                await _dbContext.Office365Subscriptions.FirstOrDefaultAsync(s =>
                    s.Office365SubscriptionId == office365SubscriptionId);

            if (subscription == null)
                throw new Exception($"Supscription office36id {office365SubscriptionId} does not exist");

            subscription.Suspended = true;
            subscription.Quantity = 1;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnsuspendSubscription(string office365SubscriptionId)
        {
            var subscription =
                await _dbContext.Office365Subscriptions.FirstOrDefaultAsync(s =>
                    s.Office365SubscriptionId == office365SubscriptionId);

            if (subscription == null)
                throw new Exception($"Supscription office36id {office365SubscriptionId} does not exist");

            subscription.Suspended = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionList(List<string> office365SubscriptionIds)
        {
            var subscriptions = await GetSubscriptionListEntity(office365SubscriptionIds);

            _dbContext.Office365Subscriptions.RemoveRange(subscriptions);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SuspendSubscriptionList(List<string> office365SubscriptionIds)
        {
            var subscriptions = await GetSubscriptionListEntity(office365SubscriptionIds);

            if (subscriptions == null || subscriptions.Count == 0)
                throw new Exception($"Supscription office36id {string.Join(",", office365SubscriptionIds)} does not exist");
            subscriptions.ForEach(x => { x.Suspended = true; x.Quantity = 1; });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UnsuspendSubscriptionList(List<string> office365SubscriptionIds)
        {
            var subscriptions = await GetSubscriptionListEntity(office365SubscriptionIds);

            if (subscriptions == null || subscriptions.Count == 0)
                throw new Exception($"Subscriptions office36id {string.Join(",", office365SubscriptionIds)} does not exist");

            subscriptions.ForEach(x => { x.Suspended = false; });
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionByProductIdentifierAsync(
            string office365CustomerId, string cloudplusProductIdentifier)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .Include(o => o.Office365Offer)
                .Include(c => c.Office365Customer)
                .FirstOrDefaultAsync(s => s.Office365Customer.Office365Id == office365CustomerId &&
                                          s.Office365Offer.CloudPlusProductIdentifier == cloudplusProductIdentifier);

            _dbContext.Office365Subscriptions.Remove(subscription);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDatabseSubscriptionState(Office365SubscriptionState subscriptionState, string office365SubscriptionId)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .FirstOrDefaultAsync(s => s.Office365SubscriptionId == office365SubscriptionId);

            if (subscription == null)
                throw new NullReferenceException($"Could not change subscription state to {subscriptionState} becaues subscription {office365SubscriptionId} was not found");

            subscription.SubscriptionState = subscriptionState;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateMultiDatabseSubscriptionState(Office365SubscriptionState subscriptionState, List<string> office365SubscriptionIds)
        {
            var subscriptions = await _dbContext.Office365Subscriptions
                .Where(s => office365SubscriptionIds.Any(x => x == s.Office365SubscriptionId)).ToListAsync();

            if (subscriptions == null || subscriptions.Count == 0)
                throw new NullReferenceException($"Could not change subscription state to {subscriptionState} because subscription {string.Join(",", office365SubscriptionIds)} was not found");

            subscriptions.ForEach(x => x.SubscriptionState = subscriptionState);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionQuantity(string office365SubscriptionId, int quantityChange)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .FirstOrDefaultAsync(s => s.Office365SubscriptionId == office365SubscriptionId);

            if(subscription == null)
                throw new NullReferenceException($"Could not change subscription quantity becaues subscription {office365SubscriptionId} was not found");
            subscription.Quantity += quantityChange;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<Office365Subscription> GetSubscriptionEntity(string office365SubscriptionId)
        {
            var subscription = await _dbContext.Office365Subscriptions
                .FirstOrDefaultAsync(s => s.Office365SubscriptionId == office365SubscriptionId);

            if (subscription == null)
                throw new ArgumentException($"No Office 365 Subscription with subscription Id: {office365SubscriptionId}");

            return subscription;
        }

        private async Task<List<Office365Subscription>> GetSubscriptionListEntity(List<string> office365SubscriptionIds)
        {
            var subscriptions = await _dbContext.Office365Subscriptions
                .Where(s => office365SubscriptionIds.Any(o => o.Contains(s.Office365SubscriptionId))).ToListAsync();

            if (subscriptions == null || subscriptions.Count == 0)
                throw new ArgumentException($"No Office 365 Subscription with subscription Id: {string.Join(",", office365SubscriptionIds)}");

            return subscriptions;
        }
    }
}
