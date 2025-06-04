using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IOrder
    {
        OrderResp PlaceOrder(OrderData orderData, int userId);

        OrderMinimal GetOrderById(int orderId);

        OrderResp CancelUnpaidOrders(int userId);
    }
}
