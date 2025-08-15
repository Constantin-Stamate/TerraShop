using System.Threading.Tasks;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class SubscriptionTests
    {
        private readonly ISubscription _subscription;

        public SubscriptionTests()
        {
            var bl = new BusinessLogicManager();
            _subscription = bl.GetSubscriptionBL();
        }

        [TestMethod]
        public async Task CreateSubscriptionSuccess()
        {
            string email = "testuser@example.com";

            var result = await _subscription.CreateSubscription(email);

            Assert.IsNotNull(result, "Expected a response object!");
            Assert.IsTrue(result.Status, "Expected subscription to be created successfully!");
            Assert.AreEqual("Subscription created successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task CreateSubscriptionEmptyEmail()
        {
            string email = "";

            var result = await _subscription.CreateSubscription(email);

            Assert.IsFalse(result.Status, "Expected failure for empty email!");
            Assert.AreEqual("Email cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public async Task CreateSubscriptionAlreadySubscribed()
        {
            string email = "existinguser@example.com";

            var result = await _subscription.CreateSubscription(email);

            Assert.IsFalse(result.Status, "Expected failure for existing email!");
            Assert.AreEqual("Email is already subscribed!", result.StatusMsg);
        }
    }
}
