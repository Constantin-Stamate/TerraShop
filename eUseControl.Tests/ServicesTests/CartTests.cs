using System.Threading.Tasks;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class CartTests
    {
        private readonly ICart _cart;

        public CartTests()
        {
            var bl = new BusinessLogicManager();
            _cart = bl.GetCartBL();
        }

        [TestMethod]
        public async Task AddNewProduct()
        {
            int productId = 3;
            int userId = 1;

            var result = await _cart.AddItemToCart(productId, userId);

            Assert.IsNotNull(result, "Expected a response object when adding a new product!");
            Assert.IsTrue(result.Status, "Expected product to be added successfully!");
        }

        [TestMethod]
        public async Task AddExistingProduct()
        {
            int productId = 1;
            int userId = 1;

            var result = await _cart.AddItemToCart(productId, userId);

            Assert.IsFalse(result.Status, "Expected failure when adding existing product!");
            Assert.AreEqual("Product is already in the cart!", result.StatusMsg);
        }

        [TestMethod]
        public async Task AddNonexistentProduct()
        {
            int productId = -1;
            int userId = 3;

            var result = await _cart.AddItemToCart(productId, userId);

            Assert.IsFalse(result.Status, "Expected failure when adding a nonexistent product!");
            Assert.AreEqual("The requested product was not found!", result.StatusMsg);
        }

        [TestMethod]
        public async Task EmptyCartList()
        {
            int userIdWithoutItems = -1;

            var result = await _cart.GetCartItemsByUserId(userIdWithoutItems);

            Assert.IsNotNull(result, "Expected an empty list instead of null!");
            Assert.AreEqual(0, result.Count, "Expected zero items for invalid user!");
        }

        [TestMethod]
        public async Task CartListFilled()
        {
            int userIdWithItems = 1;

            var result = await _cart.GetCartItemsByUserId(userIdWithItems);

            Assert.IsNotNull(result, "Expected cart items, got null!");
            Assert.IsTrue(result.Count > 0, "Expected cart to contain items!");
        }

        [TestMethod]
        public async Task RemoveValidItem()
        {
            int userId = 1;
            int productId = 1;

            var result = await _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result, "Expected a response object when removing item!");
            Assert.IsTrue(result.Status, "Expected item to be removed successfully!");
        }

        [TestMethod]
        public async Task RemoveMissingItem()
        {
            int userId = 1;
            int productId = -1;

            var result = await _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result, "Expected a response even for missing item!");
            Assert.IsFalse(result.Status, "Expected failure when removing nonexistent item!");
        }

        [TestMethod]
        public async Task RemoveInvalidUser()
        {
            int userId = -1;
            int productId = 2;

            var result = await _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result, "Expected a result even with invalid user!");
            Assert.IsFalse(result.Status, "Expected failure with invalid user ID!");
        }

        [TestMethod]
        public void GetCountExistingUser()
        {
            int userId = 1;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.IsTrue(result >= 0, "Expected cart count to be non-negative!");
        }

        [TestMethod]
        public void GetCountNoItems()
        {
            int userId = 5;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.AreEqual(0, result, "Expected zero items in cart!");
        }

        [TestMethod]
        public void GetCountInvalidUser()
        {
            int userId = -1;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.AreEqual(0, result, "Expected zero for invalid user ID!");
        }

        [TestMethod]
        public async Task ChangeQuantityNoProduct()
        {
            int userId = 1;
            int productId = -1;
            int newQuantity = 3;

            var result = await _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsFalse(result.Status, "Expected failure for invalid product ID!");
            Assert.AreEqual("The requested product was not found!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeQuantityNotInCart()
        {
            int userId = 1;
            int productId = 5;
            int newQuantity = 2;

            var result = await _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsFalse(result.Status, "Expected failure when product not in cart!");
            Assert.AreEqual("The requested item is not in your cart!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeQuantitySuccess()
        {
            int userId = 1;
            int productId = 1;
            int newQuantity = 5;

            var result = await _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsTrue(result.Status, "Expected successful quantity update!");
            Assert.AreEqual("The quantity has been successfully updated!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ApplyCouponValid()
        {
            decimal totalPrice = 100m;
            string validCoupon = "WELCOME10";

            var discountedPrice = await _cart.ApplyCouponDiscount(totalPrice, validCoupon);

            Assert.IsTrue(discountedPrice < totalPrice, "Expected discounted price to be less than total!");
            Assert.AreEqual(90m, discountedPrice, "Expected 10% discount!");
        }

        [TestMethod]
        public async Task ApplyCouponInvalid()
        {
            decimal totalPrice = 100m;
            string invalidCoupon = "INVALIDCODE";

            var discountedPrice = await _cart.ApplyCouponDiscount(totalPrice, invalidCoupon);

            Assert.AreEqual(totalPrice, discountedPrice, "Expected no discount for invalid coupon!");
        }

        [TestMethod]
        public async Task ApplyCouponExpired()
        {
            decimal totalPrice = 100m;
            string expiredCoupon = "EXPIRED15";

            var discountedPrice = await _cart.ApplyCouponDiscount(totalPrice, expiredCoupon);

            Assert.AreEqual(totalPrice, discountedPrice, "Expected no discount for expired coupon!");
        }

        [TestMethod]
        public async Task ClearCartItemsSuccess()
        {
            int userId = 2;

            var response = await _cart.ClearCartItemsAfterOrder(userId);

            Assert.IsTrue(response.Status, "Expected cart to be cleared!");
            Assert.AreEqual("Cart items cleared successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void ComputeOrderTotalValid()
        {
            decimal finalPrice = 100m;
            decimal shippingCost = 15m;

            decimal result = _cart.ComputeOrderTotal(finalPrice, shippingCost);

            Assert.AreEqual(115m, result, "Expected total to be sum of final price and shipping!");
        }

        [TestMethod]
        public void ComputeDiscountAmountValid()
        {
            decimal initialPrice = 150m;
            decimal finalPrice = 120m;

            decimal discount = _cart.ComputeDiscountAmount(initialPrice, finalPrice);

            Assert.AreEqual(30m, discount, "Expected correct discount amount!");
        }

        [TestMethod]
        public async Task CalculateCartTotal()
        {
            int userId = 1;

            var cartItems = await _cart.GetCartItemsByUserId(userId);
            var (totalPrice, shippingCost) = _cart.CalculateCartTotal(cartItems);

            Assert.IsNotNull(cartItems, "Expected non-null cart items!");
            Assert.IsTrue(totalPrice >= 0, "Expected total price to be non-negative!");
            Assert.IsTrue(shippingCost >= 0, "Expected shipping cost to be non-negative!");
        }
    }
}
