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
        public void AddNewProduct()
        {
            int productId = 3;
            int userId = 1;

            var result = _cart.AddItemToCart(productId, userId); 

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status); 
        }

        [TestMethod]
        public void AddExistingProduct()
        {
            int productId = 1;
            int userId = 1;

            var result = _cart.AddItemToCart(productId, userId); 

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Product is already in the cart!", result.StatusMsg);
        }

        [TestMethod]
        public void AddNonexistentProduct()
        {
            int productId = -1;
            int userId = 3;

            var result = _cart.AddItemToCart(productId, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("The requested product was not found!", result.StatusMsg);
        }

        [TestMethod]
        public void EmptyCartList()
        {
            int userIdWithoutItems = -1;

            var result = _cart.GetCartItemsByUserId(userIdWithoutItems);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CartListFilled()
        {
            int userIdWithItems = 1;

            var result = _cart.GetCartItemsByUserId(userIdWithItems);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveValidItem()
        {
            int userId = 1;
            int productId = 1;

            var result = _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status);
        }

        [TestMethod]
        public void RemoveMissingItem()
        {
            int userId = 1;
            int productId = -1;

            var result = _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public void RemoveInvalidUser()
        {
            int userId = -1; 
            int productId = 2;

            var result = _cart.RemoveItemFromCart(productId, userId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Status);
        }

        [TestMethod]
        public void GetCountExistingUser()
        {
            int userId = 1;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.IsTrue(result >= 0);
        }

        [TestMethod]
        public void GetCountNoItems()
        {
            int userId = 5;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetCountInvalidUser()
        {
            int userId = -1;

            int result = _cart.GetCartCountByUserId(userId);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ChangeQuantityNoProduct()
        {
            int userId = 1;
            int productId = -1;
            int newQuantity = 3;

            var result = _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("The requested product was not found!", result.StatusMsg);
        }

        [TestMethod]
        public void ChangeQuantityNotInCart()
        {
            int userId = 1;
            int productId = 5; 
            int newQuantity = 2;

            var result = _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("The requested item is not in your cart!", result.StatusMsg);
        }

        [TestMethod]
        public void ChangeQuantitySuccess()
        {
            int userId = 1;
            int productId = 1;
            int newQuantity = 5;

            var result = _cart.ChangeProductQuantity(productId, userId, newQuantity);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("The quantity has been successfully updated!", result.StatusMsg);
        }

        [TestMethod]
        public void ApplyCouponValid()
        {
            decimal totalPrice = 100m;
            string validCoupon = "WELCOME10";

            var discountedPrice = _cart.ApplyCouponDiscount(totalPrice, validCoupon);

            Assert.IsTrue(discountedPrice < totalPrice);
            Assert.AreEqual(90m, discountedPrice);
        }

        [TestMethod]
        public void ApplyCouponInvalid()
        {
            decimal totalPrice = 100m;
            string invalidCoupon = "INVALIDCODE";

            var discountedPrice = _cart.ApplyCouponDiscount(totalPrice, invalidCoupon);

            Assert.AreEqual(totalPrice, discountedPrice);
        }

        [TestMethod]
        public void ApplyCouponExpired()
        {
            decimal totalPrice = 100m;
            string expiredCoupon = "EXPIRED15";

            var discountedPrice = _cart.ApplyCouponDiscount(totalPrice, expiredCoupon);

            Assert.AreEqual(totalPrice, discountedPrice);
        }

        [TestMethod]
        public void ClearCartItemsSuccess()
        {
            int userId = 2; 

            var response = _cart.ClearCartItemsAfterOrder(userId);

            Assert.IsTrue(response.Status);
            Assert.AreEqual("Cart items cleared successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void ComputeOrderTotalValid()
        {
            decimal finalPrice = 100m;
            decimal shippingCost = 15m;

            decimal result = _cart.ComputeOrderTotal(finalPrice, shippingCost);

            Assert.AreEqual(115m, result);
        }

        [TestMethod]
        public void ComputeDiscountAmountValid()
        {
            decimal initialPrice = 150m;
            decimal finalPrice = 120m;

            decimal discount = _cart.ComputeDiscountAmount(initialPrice, finalPrice);

            Assert.AreEqual(30m, discount);
        }

        [TestMethod]
        public void CalculateCartTotal()
        {
            int userId = 1;

            var cartItems = _cart.GetCartItemsByUserId(userId);
            var (totalPrice, shippingCost) = _cart.CalculateCartTotal(cartItems);

            Assert.IsNotNull(cartItems);
            Assert.IsTrue(totalPrice >= 0);
            Assert.IsTrue(shippingCost >= 0);
        }
    }
}
