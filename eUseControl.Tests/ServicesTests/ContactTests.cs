using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Contact;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ContactTests
    {
        private readonly IContact _contact;

        public ContactTests()
        {
            var bl = new BusinessLogicManager();
            _contact = bl.GetContactBL();
        }

        [TestMethod]
        public void SubmitUsernameMissing()
        {
            int userId = 1;
            var data = new ContactData
            {
                Username = "",
                Email = "test@mail.com",
                Message = "Hello"
            };

            var result = _contact.SubmitContactRequest(data, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false when username is missing!");
            Assert.AreEqual("Please complete all required fields!", result.StatusMsg);
        }

        [TestMethod]
        public void SubmitEmailMissing()
        {
            int userId = 1;
            var data = new ContactData
            {
                Username = "Test",
                Email = "",
                Message = "Hello"
            };

            var result = _contact.SubmitContactRequest(data, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false when email is missing!");
            Assert.AreEqual("Please complete all required fields!", result.StatusMsg);
        }

        [TestMethod]
        public void SubmitMessageMissing()
        {
            int userId = 1;
            var data = new ContactData
            {
                Username = "Test",
                Email = "test@mail.com",
                Message = ""
            };

            var result = _contact.SubmitContactRequest(data, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false when message is missing!");
            Assert.AreEqual("Please complete all required fields!", result.StatusMsg);
        }

        [TestMethod]
        public void SubmitValidRequest()
        {
            int userId = 1;
            var data = new ContactData
            {
                Username = "ValidUser",
                Email = "validuser@example.com",
                Message = "This is a valid message."
            };

            var result = _contact.SubmitContactRequest(data, userId);

            Assert.IsTrue(result.Status, "Expected Status to be true for valid request!");
            Assert.AreEqual("Contact request submitted successfully!", result.StatusMsg);
        }
    }
}
