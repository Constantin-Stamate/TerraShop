using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class OrderBL : UserApi, IOrder
    {
        public async Task<OrderResp> PlaceOrder(OrderData orderData, int userId)
        {
            return await PlaceOrderAction(orderData, userId);
        }

        public async Task<OrderMinimal> GetOrderById(int orderId)
        {
            return await GetOrderByIdAction(orderId);
        }

        public async Task<OrderResp> CancelUnpaidOrders(int userId)
        {
            return await CancelUnpaidOrdersAction(userId);
        }

        public async Task<List<OrderLite>> GetValidOrders(int userId)
        {
            return await GetValidOrdersAction(userId);
        }

        public async Task<OrderResp> CancelOrder(int orderId)
        {
            return await CancelOrderAction(orderId);
        }
    }
}
