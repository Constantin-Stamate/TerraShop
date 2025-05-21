using System.Collections.Generic;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ICart
    {
        CartResp AddItemToCart(int productId, int userId);

        List<CartData> GetCartItemsByUserId(int userId);

        CartResp RemoveItemFromCart(int productId, int userId);

        int GetCartCountByUserId(int userId);

        CartResp ChangeProductQuantity(int productId, int userId, int newQuantity);

        decimal CalculateCartTotal(List<CartData> cartItems);

        decimal ApplyCouponDiscount(decimal totalPrice, string couponCode);
    }
}
