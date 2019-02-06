using System.Collections.Generic;
using System.Threading.Tasks;
using CloudPlus.Models.Office365.Order;

// ReSharper disable once CheckNamespace
namespace CloudPlus.Services.Office365.OrderService
{
    public interface IOffice365OrderService
    {
        Task<Office365OrderModel> CreateOrderAsync(Office365OrderModel orderModel);

        Task<Office365OrderWithDetailsModel> CreateOrderWithMultiItemsAsync(Office365OrderWithDetailsModel orderModel);
    }
}
