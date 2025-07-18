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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsFalse(result.Status);
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

            Assert.IsTrue(result.Status);
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

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Your order has been placed successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void OrderNotFound()
        {
            int orderId = -1;

            var result = _order.GetOrderById(orderId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void OrderFound()
        {
            int orderId = 1;

            var result = _order.GetOrderById(orderId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CancelUnpaidOrdersSuccess()
        {
            int userId = 1;

            var response = _order.CancelUnpaidOrders(userId);

            Assert.IsTrue(response.Status);
            Assert.AreEqual("Unpaid orders have been cancelled successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void GetValidOrdersValidUser()
        {
            int userId = 1;

            var result = _order.GetValidOrders(userId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetValidOrdersInvalidUser()
        {
            int invalidUserId = -1;

            var result = _order.GetValidOrders(invalidUserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CancelOrderValidId()
        {
            int orderId = 1;

            var result = _order.CancelOrder(orderId);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Order cancelled successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void CancelOrderInvalidId()
        {
            int invalidOrderId = -1;

            var result = _order.CancelOrder(invalidOrderId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Order not found!", result.StatusMsg);
        }
    }
}
