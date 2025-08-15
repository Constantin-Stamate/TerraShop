using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ICart
    {
        Task<CartResp> AddItemToCart(int productId, int userId);

        Task<List<CartData>> GetCartItemsByUserId(int userId);

        Task<CartResp> RemoveItemFromCart(int productId, int userId);

        int GetCartCountByUserId(int userId);

        Task<CartResp> ChangeProductQuantity(int productId, int userId, int newQuantity);

        (decimal totalPrice, decimal shippingCost) CalculateCartTotal(List<CartData> cartItems);

        Task<decimal> ApplyCouponDiscount(decimal totalPrice, string couponCode);

        Task<CartResp> ClearCartItemsAfterOrder(int userId);

        decimal ComputeOrderTotal(decimal finalPrice, decimal shippingCost);

        decimal ComputeDiscountAmount(decimal initialPrice, decimal finalPrice);
    }
}
