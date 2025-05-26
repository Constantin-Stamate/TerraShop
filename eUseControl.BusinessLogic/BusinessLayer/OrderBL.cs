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
    }
}
