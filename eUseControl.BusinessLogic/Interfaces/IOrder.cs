using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IOrder
    {
        Task<OrderResp> PlaceOrder(OrderData orderData, int userId);

        Task<OrderMinimal> GetOrderById(int orderId);

        Task<OrderResp> CancelUnpaidOrders(int userId);

        Task<List<OrderLite>> GetValidOrders(int userId);

        Task<OrderResp> CancelOrder(int orderId);
    }
}
