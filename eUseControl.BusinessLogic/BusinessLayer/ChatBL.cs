using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Chat;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ChatBL : UserApi, IChat
    {
        public async Task<ChatResp> GetResponse(string message, int userId)
        {
            return await GetResponseAction(message, userId);
        }

        public async Task<List<ChatData>> RetrieveUserChats(int userId)
        {
            return await RetrieveUserChatsAction(userId);
        }

        public async Task<ChatResp> DeleteChatHistory(int userId)
        {
            return await DeleteChatHistoryAction(userId);
        }
    }
}
