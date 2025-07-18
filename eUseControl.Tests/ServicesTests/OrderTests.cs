using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class OrderTests
    {
        private readonly IOrder _order;

        public OrderTests()
        {
            var bl = new BusinessLogicManager();
            _order = bl.GetOrderBL();
        }

        [TestMethod]
        public void PlaceOrderMissingFields()
        {
            int userId = 1;
            var orderData = new OrderData
            {
                FirstName = "",
                LastName = "Smith",
                DeliveryAddress = "123 Street",
                PhoneNumber = "+1234567890",
                Email = "test@example.com",
                PaymentMethod = "CreditCard",
                TotalPrice = 100
            };

            var result = _order.PlaceOrder(orderData, userId);

            Assert.IsFalse(result.Status, "Expected failure due to missing required fields!");
            Assert.AreEqual("Please complete all required fields!", result.StatusMsg);
        }

        [TestMethod]
        public void PlaceOrderInvalidEmail()
        {
            int userId = 1;
            var orderData = new OrderData
            {
                FirstName = "John",
                LastName = "Smith",
                DeliveryAddress = "123 Street",
                PhoneNumber = "+1234567890",
                Email = "invalid-email",
                PaymentMethod = "CreditCard",
                TotalPrice = 100
            };

            var result = _order.PlaceOrder(orderData, userId);

            Assert.IsFalse(result.Status, "Expected failure due to invalid email format!");
            Assert.AreEqual("Please enter a valid email address!", result.StatusMsg);
        }

        [TestMethod]
        public void PlaceOrderInvalidPhone()
        {
            int userId = 1;
            var orderData = new OrderData
            {
                FirstName = "John",
                LastName = "Smith",
                DeliveryAddress = "123 Street",
                PhoneNumber = "123-abc",
                Email = "test@example.com",
                PaymentMethod = "CreditCard",
                TotalPrice = 100
            };

            var result = _order.PlaceOrder(orderData, userId);

            Assert.IsFalse(result.Status, "Expected failure due to invalid phone number format!");
            Assert.AreEqual("Please enter a valid phone number!", result.StatusMsg);
        }

        [TestMethod]
        public void PlaceOrderValidDataWithoutCoupon()
        {
            int userId = 1;
            var orderData = new OrderData
            {
                FirstName = "John",
                LastName = "Smith",
                DeliveryAddress = "123 Street",
                PhoneNumber = "+1234567890",
                Email = "test@example.com",
                PaymentMethod = "CreditCard",
                TotalPrice = 100,
                CouponCode = null
            };

            var result = _order.PlaceOrder(orderData, userId);

            Assert.IsTrue(result.Status, "Expected order placement to succeed without a coupon!");
            Assert.AreEqual("Your order has been placed successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void PlaceOrderValidDataWithValidCoupon()
        {
            int userId = 1;
            var orderData = new OrderData
            {
                FirstName = "John",
                LastName = "Smith",
                DeliveryAddress = "123 Street",
                PhoneNumber = "+1234567890",
                Email = "test@example.com",
                PaymentMethod = "CreditCard",
                TotalPrice = 90,
                CouponCode = "DISCOUNT10"
            };

            var result = _order.PlaceOrder(orderData, userId);

            Assert.IsTrue(result.Status, "Expected order placement to succeed with a valid coupon!");
            Assert.AreEqual("Your order has been placed successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void OrderNotFound()
        {
            int orderId = -1;

            var result = _order.GetOrderById(orderId);

            Assert.IsNull(result, "Expected null result when order ID is invalid!");
        }

        [TestMethod]
        public void OrderFound()
        {
            int orderId = 1;

            var result = _order.GetOrderById(orderId);

            Assert.IsNotNull(result, "Expected to find an order with the given valid ID!");
        }

        [TestMethod]
        public void CancelUnpaidOrdersSuccess()
        {
            int userId = 1;

            var response = _order.CancelUnpaidOrders(userId);

            Assert.IsTrue(response.Status, "Expected unpaid orders to be cancelled successfully!");
            Assert.AreEqual("Unpaid orders have been cancelled successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void GetValidOrdersValidUser()
        {
            int userId = 1;

            var result = _order.GetValidOrders(userId);

            Assert.IsNotNull(result, "Expected non-null list of valid orders for the user!");
            Assert.IsTrue(result.Count > 0, "Expected at least one valid order for the user!");
        }

        [TestMethod]
        public void GetValidOrdersInvalidUser()
        {
            int invalidUserId = -1;

            var result = _order.GetValidOrders(invalidUserId);

            Assert.IsNotNull(result, "Expected a non-null list even for an invalid user!");
            Assert.AreEqual(0, result.Count, "Expected the result list to be empty for an invalid user ID!");
        }

        [TestMethod]
        public void CancelOrderValidId()
        {
            int orderId = 1;

            var result = _order.CancelOrder(orderId);

            Assert.IsTrue(result.Status, "Expected successful cancellation of the order!");
            Assert.AreEqual("Order cancelled successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void CancelOrderInvalidId()
        {
            int invalidOrderId = -1;

            var result = _order.CancelOrder(invalidOrderId);

            Assert.IsFalse(result.Status, "Expected failure when trying to cancel a non-existent order!");
            Assert.AreEqual("Order not found!", result.StatusMsg);
        }
    }
}
