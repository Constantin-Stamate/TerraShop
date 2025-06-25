using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class OrderBL : UserApi, IOrder
    {
        public OrderResp PlaceOrder(OrderData orderData, int userId)
        {
            return PlaceOrderAction(orderData, userId);
        }

        public OrderMinimal GetOrderById(int orderId)
        {
            return GetOrderByIdAction(orderId);
        }

        public OrderResp CancelUnpaidOrders(int userId)
        {
            return CancelUnpaidOrdersAction(userId);
        }

        public List<OrderLite> GetValidOrders(int userId)
        {
            return GetValidOrdersAction(userId);
        }

        public OrderResp CancelOrder(int orderId)
        {
            return CancelOrderAction(orderId);
        }
    }
}
