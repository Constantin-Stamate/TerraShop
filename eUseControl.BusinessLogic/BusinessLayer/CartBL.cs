using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class CartBL : UserApi, ICart
    {
        public async Task<CartResp> AddItemToCart(int productId, int userId)
        {
            return await AddItemToCartAction(productId, userId);
        }

        public async Task<List<CartData>> GetCartItemsByUserId(int userId)
        {
            return await GetCartItemsByUserIdAction(userId);
        }

        public async Task<CartResp> RemoveItemFromCart(int productId, int userId)
        {
            return await RemoveItemFromCartAction(productId, userId);
        }

        public int GetCartCountByUserId(int userId)
        {
            return GetCartCountByUserIdAction(userId);
        }

        public async Task<CartResp> ChangeProductQuantity(int productId, int userId, int newQuantity)
        {
            return await ChangeProductQuantityAction(productId, userId, newQuantity);
        }

        public (decimal totalPrice, decimal shippingCost) CalculateCartTotal(List<CartData> cartItems)
        {
            return CalculateCartTotalAction(cartItems);
        }

        public async Task<decimal> ApplyCouponDiscount(decimal totalPrice, string couponCode)
        {
            return await ApplyCouponDiscountAction(totalPrice, couponCode);
        }

        public async Task<CartResp> ClearCartItemsAfterOrder(int userId)
        {
            return await ClearCartItemsAfterOrderAction(userId);
        }

        public decimal ComputeOrderTotal(decimal finalPrice, decimal shippingCost)
        {
            return ComputeOrderTotalAction(finalPrice, shippingCost);
        }

        public decimal ComputeDiscountAmount(decimal initialPrice, decimal finalPrice)
        {
            return ComputeDiscountAmountAction(initialPrice, finalPrice);
        }
    }
}
