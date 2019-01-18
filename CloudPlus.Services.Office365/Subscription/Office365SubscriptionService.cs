using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPlus.Logging;
using Microsoft.Store.PartnerCenter.Models.Subscriptions;
using CloudPlus.Models.Office365.Order;
using CloudPlus.Models.Office365.Subscription;
using CloudPlus.Resources;
using CloudPlus.Services.Office365.Operations;
using CloudPlus.Services.Office365.OrderService;

namespace CloudPlus.Services.Office365.Subscription
{
    public class Office365SubscriptionService : IOffice365SubscriptionService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly IOffice365OrderService _office365OrderService;
        private readonly int _retryAttempts;

        public Office365SubscriptionService(
            IPartnerOperations partnerOperations,
            IOffice365OrderService office365OrderService,
            IConfigurationManager configurationManager)
        {
            _partnerOperations = partnerOperations;
            _office365OrderService = office365OrderService;
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task<Office365SubscriptionModel> GetPartnerPlatformSubscriptionByOfferAsync(string office365CustomerId, string office365OfferId)
        {
            var allSubscriptions = await _partnerOperations.UserPartnerOperations.Customers
                .ById(office365CustomerId)
                .Subscriptions.GetAsync();

            if (!allSubscriptions.Items.Any()) return null;

            var existingSubscription = allSubscriptions.Items.FirstOrDefault(s => s.OfferId == office365OfferId);

            if (existingSubscription == null) return null;

            return new Office365SubscriptionModel
            {
                Office365CustomerId = office365CustomerId,
                Office365SubscriptionId = existingSubscription.Id,
                Office365OrderId = existingSubscription.OrderId,
                Office365FriendlyName = existingSubscription.FriendlyName,
                Quantity = existingSubscription.Quantity
            };
        }

        public async Task<Office365SubscriptionModel> CreatePartnerPlatformSubscriptionAsync(string office365CustomerId, string office365OfferId, int quantity = 1)
        {
            var orderModel = new Office365OrderModel
            {
                Office365CustomerId = office365CustomerId,
                Office365OfferId = office365OfferId,
                Quantity = quantity
            };

            var placedOrder = await _office365OrderService.CreateOrderAsync(orderModel);

            return new Office365SubscriptionModel
            {
                Office365CustomerId = office365CustomerId,
                Office365SubscriptionId = placedOrder.SubscriptionId,
                Office365OrderId = placedOrder.OrderId,
                Office365FriendlyName = placedOrder.FriendlyName,
                Quantity = placedOrder.Quantity
            };
        }

        public async Task<Office365SubscriptionModel> ActivateSuspendedPartnerPlatformSubscriptionAsync(string office365CustomerId, string office365SubscriptionId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var existingSubscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).GetAsync();

                    existingSubscription.Status = SubscriptionStatus.Active;

                    var subscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).PatchAsync(existingSubscription);

                    await ConfirmSubscriptionStatusUpdate(office365CustomerId, subscription.Id, SubscriptionStatus.Active);

                    requestSuccess = true;

                    return new Office365SubscriptionModel
                    {
                        Office365CustomerId = office365CustomerId,
                        Office365SubscriptionId = subscription.Id,
                        Office365OrderId = subscription.OrderId,
                        Office365FriendlyName = subscription.FriendlyName,
                        Quantity = subscription.Quantity
                    };
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Activate suspended subscription request failed request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Activate suspended subscription request failed!");
        }

        public async Task SuspendPartnerPlatformSubscriptionAsync(string office365CustomerId, string office365SubscriptionId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var existingSubscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).GetAsync();

                    existingSubscription.Status = SubscriptionStatus.Suspended;

                    await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).PatchAsync(existingSubscription);

                    await ConfirmSubscriptionStatusUpdate(office365CustomerId, office365SubscriptionId, SubscriptionStatus.Suspended);

                    requestSuccess = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Suspend subscription request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            if (!requestSuccess) throw new Exception("Suspended subscription request failed!");
        }

        public async Task<Office365SubscriptionModel> IncreasePartnerPlatformSubscriptionQuantityAsync(string office365CustomerId, string office365SubscriptionId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var existingSubscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).GetAsync();

                    existingSubscription.Quantity++;

                    var subscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).PatchAsync(existingSubscription);

                    await ConfirmSubscriptionQuantityUpdate(office365CustomerId, office365SubscriptionId,
                        existingSubscription.Quantity);

                    requestSuccess = true;

                    return new Office365SubscriptionModel
                    {
                        Office365CustomerId = office365CustomerId,
                        Office365SubscriptionId = subscription.Id,
                        Office365OrderId = subscription.OrderId,
                        Office365FriendlyName = subscription.FriendlyName,
                        Quantity = subscription.Quantity
                    };
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Increase partner platform subscription quantity request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Increase partner platform subscription quantity");
        }

        public async Task<Office365SubscriptionModel> UpdatePartnerPlatformSubscriptionQuantity(string office365CustomerId, string office365SubscriptionId, int quantityChange)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var existingSubscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).GetAsync();

                    existingSubscription.Quantity += quantityChange;

                    var subscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).PatchAsync(existingSubscription);

                    await ConfirmSubscriptionQuantityUpdate(office365CustomerId, office365SubscriptionId,
                        existingSubscription.Quantity);

                    requestSuccess = true;

                    return new Office365SubscriptionModel
                    {
                        Office365CustomerId = office365CustomerId,
                        Office365SubscriptionId = subscription.Id,
                        Office365OrderId = subscription.OrderId,
                        Office365FriendlyName = subscription.FriendlyName,
                        Quantity = subscription.Quantity
                    };
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Update partner platform subscription quantity request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Update partner platform subscription quantity");
        }

        public async Task<Office365SubscriptionModel> DecreasePartnerPlatformSubscriptionQuantityAsync(string office365CustomerId, string office365SubscriptionId)
        {
            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var existingSubscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).GetAsync();

                    if (existingSubscription.Quantity == 1)
                    {
                        existingSubscription.Status = SubscriptionStatus.Suspended;
                        existingSubscription.SuspensionReasons = new List<string>
                        {
                            "There are no more licences that customer uses"
                        };
                    }
                    else
                        existingSubscription.Quantity--;

                    var subscription = await _partnerOperations.UserPartnerOperations.Customers.ById(office365CustomerId)
                        .Subscriptions.ById(office365SubscriptionId).PatchAsync(existingSubscription);

                    await ConfirmSubscriptionQuantityUpdate(office365CustomerId, office365SubscriptionId,
                        existingSubscription.Quantity);

                    requestSuccess = true;

                    return new Office365SubscriptionModel
                    {
                        Office365CustomerId = office365CustomerId,
                        Office365SubscriptionId = subscription.Id,
                        Office365OrderId = subscription.OrderId,
                        Office365FriendlyName = subscription.FriendlyName,
                        Quantity = subscription.Quantity
                    };
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Decrease partner platform subscription quantity request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Decrease partner platform subscription quantity");
        }

        private async Task ConfirmSubscriptionStatusUpdate(string office365CustomerId, string office365SubscriptionId, SubscriptionStatus status)
        {
            var subscriptionActivated = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the subscription with id {office365SubscriptionId} to activate/suspend. Take {attempts}");

                    var subscription = await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Subscriptions.ById(office365SubscriptionId).GetAsync();

                    if (subscription.Status != status)
                        throw new NullReferenceException($"Subscription with id {office365SubscriptionId} is not activated/suspended yet");

                    subscriptionActivated = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!subscriptionActivated && attempts < _retryAttempts);

            if (!subscriptionActivated) throw new Exception("Could not confirm updating subscription status!");
        }

        private async Task ConfirmSubscriptionQuantityUpdate(string office365CustomerId, string office365SubscriptionId, int quantity)
        {
            var subscriptionUpdated = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the subscription with id {office365SubscriptionId} to update quantity. Take {attempts}");

                    var subscription = await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Subscriptions.ById(office365SubscriptionId).GetAsync();

                    if (subscription.Quantity != quantity)
                        throw new Exception($"Subscription with id {office365SubscriptionId} is not updated yet");

                    subscriptionUpdated = true;
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!subscriptionUpdated && attempts < _retryAttempts);

            if (!subscriptionUpdated) throw new Exception("Could not confirm updating subscription quantity!");
        }
    }
}
