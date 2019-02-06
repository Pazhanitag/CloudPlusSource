using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Store.PartnerCenter.Models.Orders;
using CloudPlus.Logging;
using CloudPlus.Models.Office365.Order;
using CloudPlus.Resources;
using CloudPlus.Services.Office365.Operations;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.OrderService
{
    public class Office365OrderService : IOffice365OrderService
    {
        private readonly IPartnerOperations _partnerOperations;
        private readonly int _retryAttempts;

        public Office365OrderService(IPartnerOperations partnerOperations, IConfigurationManager configurationManager)
        {
            _partnerOperations = partnerOperations;
            _retryAttempts = int.Parse(configurationManager.GetByKey("RetryAttempts"));
        }

        public async Task<Office365OrderModel> CreateOrderAsync(Office365OrderModel orderModel)
        {
            var newOrder = new Order()
            {
                ReferenceCustomerId = orderModel.Office365CustomerId,
                LineItems = new List<OrderLineItem>
                {
                    new OrderLineItem
                    {
                        OfferId = orderModel.Office365OfferId,
                        FriendlyName = orderModel.FriendlyName,
                        Quantity = orderModel.Quantity
                    }
                }
            };

            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var createdOrder = await _partnerOperations.UserPartnerOperations
                       .Customers.ById(orderModel.Office365CustomerId).Orders.CreateAsync(newOrder);

                    orderModel.OrderId = createdOrder.Id;
                    orderModel.SubscriptionId = createdOrder.LineItems.FirstOrDefault()?.SubscriptionId;

                    requestSuccess = true;

                    await ConfirmCreateOrder(orderModel.Office365CustomerId, orderModel.OrderId);

                    return orderModel;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Create order request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Create order request failed!");
        }

        public async Task<Office365OrderWithDetailsModel> CreateOrderWithMultiItemsAsync(Office365OrderWithDetailsModel orderModel)
        {
            var newOrder = new Order()
            {
                ReferenceCustomerId = orderModel.Office365CustomerId,
                LineItems = orderModel.Office365OrderDetailsModels.Select(x => new OrderLineItem
                {
                    OfferId = x.Office365OfferId,
                    FriendlyName = x.FriendlyName,
                    Quantity = x.Quantity
                })
            };

            var requestSuccess = false;
            var attempts = 1;
            do
            {
                try
                {
                    var createdOrder = await _partnerOperations.UserPartnerOperations
                       .Customers.ById(orderModel.Office365CustomerId).Orders.CreateAsync(newOrder);

                    orderModel.OrderId = createdOrder.Id;
                    orderModel.Office365OrderDetailsModels.ForEach(x =>
                    {
                        x.SubscriptionId = createdOrder.LineItems.Where(i => i.OfferId == x.Office365OfferId).FirstOrDefault().SubscriptionId;
                    });

                    requestSuccess = true;

                    await ConfirmCreateOrder(orderModel.Office365CustomerId, orderModel.OrderId);

                    return orderModel;
                }
                catch (Exception ex)
                {
                    this.Log().Error($"Create order request failed! Attampt: {attempts}", ex);
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!requestSuccess && attempts < _retryAttempts);

            throw new Exception("Create order with line items request failed!");
        }


        private async Task ConfirmCreateOrder(string office365CustomerId, string office365OrderId)
        {
            var orderCreated = false;
            var attempts = 1;
            do
            {
                try
                {
                    this.Log().Info($"Waiting for the order {office365OrderId} to be created. Take {attempts}");

                    await _partnerOperations.UserPartnerOperations
                        .Customers.ById(office365CustomerId).Orders.ById(office365OrderId).GetAsync();

                    orderCreated = true;
                }
                catch (Exception)
                {
                    this.Log().Error($"Failed getting order {office365OrderId}");
                    attempts++;
                    await Task.Delay(3000);
                }
            } while (!orderCreated && attempts < _retryAttempts);

            if (!orderCreated) throw new Exception("Could not confirm Order creation!");
        }
    }
}
