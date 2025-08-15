using System.Threading.Tasks;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class TransactionTests
    {
        private readonly ITransaction _transaction;

        public TransactionTests()
        {
            var bl = new BusinessLogicManager();
            _transaction = bl.GetTransactionBL();
        }

        [TestMethod]
        public async Task FailInvalidCard()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "",
                ExpiryDate = "25/12",
                Cvv = "123",
                FullName = "John Doe",
                OrderId = 1
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsFalse(resp.Status, "Expected payment to fail due to invalid card number!");
            Assert.AreEqual("The card information you entered appears to be invalid!", resp.StatusMsg);
        }

        [TestMethod]
        public async Task FailInvalidExpiryDate()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "00/00",
                Cvv = "123",
                FullName = "John Doe",
                OrderId = 1
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsFalse(resp.Status, "Expected payment to fail due to invalid expiration date!");
            Assert.AreEqual("The expiration date you entered is invalid!", resp.StatusMsg);
        }

        [TestMethod]
        public async Task FailInvalidCVV()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "25/12",
                Cvv = "abc",
                FullName = "John Doe",
                OrderId = 1
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsFalse(resp.Status, "Expected payment to fail due to invalid CVV code!");
            Assert.AreEqual("The CVV code entered is invalid!", resp.StatusMsg);
        }

        [TestMethod]
        public async Task FailInvalidFullName()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "25/12",
                Cvv = "123",
                FullName = "",
                OrderId = 1
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsFalse(resp.Status, "Expected payment to fail due to invalid full name!");
            Assert.AreEqual("The full name you entered is invalid!", resp.StatusMsg);
        }

        [TestMethod]
        public async Task FailInvalidOrderAmount()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "25/12",
                Cvv = "123",
                FullName = "John Doe",
                OrderId = 9
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsFalse(resp.Status, "Expected payment to fail because the order amount is invalid or cannot be processed!");
            Assert.AreEqual("Your payment could not be processed!", resp.StatusMsg);
        }

        [TestMethod]
        public async Task SuccessPaymentProcessed()
        {
            int userId = 1;
            var transactionData = new TransactionData
            {
                CardNumber = "4111111111111111",
                ExpiryDate = "25/12",
                Cvv = "123",
                FullName = "John Doe",
                OrderId = 1
            };

            var resp = await _transaction.ProcessPayment(transactionData, userId);

            Assert.IsTrue(resp.Status, "Expected payment to be processed successfully!");
            Assert.AreEqual("Payment was successfully completed!", resp.StatusMsg);
        }
    }
}
