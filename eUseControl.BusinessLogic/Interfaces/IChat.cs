using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Chat;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IChat
    {
        Task<ChatResp> GetResponse(string message, int userId);

        Task<List<ChatData>> RetrieveUserChats(int userId);

        Task<ChatResp> DeleteChatHistory(int userId);
    }
}
