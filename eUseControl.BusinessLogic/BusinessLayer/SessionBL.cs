using System.Web;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class SessionBL : UserApi, ISession
    {
        public URegisterResp UserRegister(URegisterData data)
        {
            return UserRegisterAction(data);
        }

        public HttpCookie GenCookie(string loginCredential)
        {
            return Cookie(loginCredential);
        }

        public UserMinimal GetUserByCookie(string apiCookieValue)
        {
            return UserCookie(apiCookieValue);
        }

        public ULoginResp UserLogin(ULoginData data)
        {
            return UserLoginAction(data);
        }
    }
}
