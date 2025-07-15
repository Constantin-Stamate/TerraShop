using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ChatBL : UserApi, IChat
    {
        public string GetResponse(string message)
        {
            return GetResponseAction(message);
        }
    }
}
