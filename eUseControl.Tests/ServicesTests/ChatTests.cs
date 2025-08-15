using System.Linq;
using System.Threading.Tasks;
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
        public async Task EmptyMessageError()
        {
            string message = "";
            int userId = 1;

            var result = await _chat.GetResponse(message, userId);

            Assert.IsFalse(result.Status, "Expected failure when message is empty!");
            Assert.AreEqual("Message cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public async Task TooLongMessageError()
        {
            string message = string.Join(" ", Enumerable.Repeat("word", 601));
            int userId = 1;

            var result = await _chat.GetResponse(message, userId);

            Assert.IsFalse(result.Status, "Expected failure when message exceeds allowed word limit!");
            Assert.IsTrue(result.StatusMsg.Contains("Message too long"));
        }

        [TestMethod]
        public async Task ValidMessageSuccess()
        {
            string message = "How does Eco Market Place work?";
            int userId = 1;

            var result = await _chat.GetResponse(message, userId);

            Assert.IsTrue(result.Status, "Expected success for valid message input!");
            Assert.IsFalse(string.IsNullOrEmpty(result.StatusMsg), "Expected a meaningful response for a valid message!");
        }

        [TestMethod]
        public async Task OutOfScopeMessageHandled()
        {
            string message = "Who won the football world cup?";
            int userId = 1;

            var result = await _chat.GetResponse(message, userId);

            Assert.IsTrue(result.Status, "Expected status to remain true even for out-of-domain questions!");
            Assert.IsTrue(result.StatusMsg.Contains("Eco Market Place") || result.StatusMsg.Contains("I can only assist") || result.StatusMsg.Contains("platform"), "Expected informative message indicating domain limitation for the AI assistant!");
        }

        [TestMethod]
        public async Task RetrieveUserChatsSuccess()
        {
            int existingUserId = 1;

            var chats = await _chat.RetrieveUserChats(existingUserId);

            Assert.IsNotNull(chats, "Expected a list of chat data!");
            Assert.IsTrue(chats.Count > 0, "Expected at least one chat entry for existing user!");
        }

        [TestMethod]
        public async Task RetrieveUserChatsFailForInvalidUser()
        {
            int nonExistentUserId = -1;

            var chats = await _chat.RetrieveUserChats(nonExistentUserId);

            Assert.IsNotNull(chats, "Expected a list even for invalid user!");
            Assert.AreEqual(0, chats.Count, "Expected empty list for non-existent user!");
        }

        [TestMethod]
        public async Task DeleteChatSuccess()
        {
            int userIdWithMessages = 1;

            var result = await _chat.DeleteChatHistory(userIdWithMessages);

            Assert.IsTrue(result.Status, "Expected true when deleting existing user messages!");
            Assert.AreEqual("User messages successfully deleted!", result.StatusMsg);
        }

        [TestMethod]
        public async Task DeleteChatFail()
        {
            int userIdWithoutMessages = -1;

            var result = await _chat.DeleteChatHistory(userIdWithoutMessages);

            Assert.IsFalse(result.Status, "Expected false when no messages exist for the user!");
            Assert.AreEqual("No messages found for this user!", result.StatusMsg);
        }
    }
}
