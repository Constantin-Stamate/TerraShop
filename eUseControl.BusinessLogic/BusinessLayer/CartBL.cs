using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class CartBL : UserApi, ICart
    {
        public CartResp AddItemToCart(int productId, int userId)
        {
            return AddItemToCartAction(productId, userId);
        }

        public List<CartData> GetCartItemsByUserId(int userId)
        {
            return GetCartItemsByUserIdAction(userId);
        }

        public CartResp RemoveItemFromCart(int productId, int userId)
        {
            return RemoveItemFromCartAction(productId, userId);
        }

        public int GetCartCountByUserId(int userId)
        {
            return GetCartCountByUserIdAction(userId);
        }

        public CartResp ChangeProductQuantity(int productId, int userId, int newQuantity)
        {
            return ChangeProductQuantityAction(productId, userId, newQuantity);
        }

        public decimal CalculateCartTotal(List<CartData> cartItems)
        {
            return CalculateCartTotalAction(cartItems);
        }

        public decimal ApplyCouponDiscount(decimal totalPrice, string couponCode)
        {
            return ApplyCouponDiscountAction(totalPrice, couponCode);
        }
    }
}
