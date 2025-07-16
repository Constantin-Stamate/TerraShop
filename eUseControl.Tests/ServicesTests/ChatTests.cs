using System.Linq;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ChatTests
    {
        private readonly IChat _chat;

        public ChatTests()
        {
            var bl = new BusinessLogicManager();
            _chat = bl.GetChatBL();
        }

        [TestMethod]
        public void EmptyMessage()
        {
            string emptyMessage = "";
            string result = _chat.GetResponse(emptyMessage);

            Assert.AreEqual("Message cannot be empty!", result);
        }

        [TestMethod]
        public void LongMessage()
        {
            string longMessage = string.Join(" ", Enumerable.Repeat("word", 301));
            string result = _chat.GetResponse(longMessage);

            Assert.IsTrue(result.Contains("Message too long."));
        }

        [TestMethod]
        public void ValidMessage()
        {
            string validMessage = "How does Eco Market Place work?";
            string result = _chat.GetResponse(validMessage);

            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsFalse(result.StartsWith("Error"));
        }

        [TestMethod]
        public void OutOfDomain()
        {
            string outOfDomainMessage = "Who won the football world cup?";
            string result = _chat.GetResponse(outOfDomainMessage);

            Assert.IsTrue(result.Contains("I can only assist with topics related to the Eco Market Place."));
        }
    }
}
