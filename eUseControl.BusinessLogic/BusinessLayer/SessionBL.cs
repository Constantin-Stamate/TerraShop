using System.Threading.Tasks;
using System.Web;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class SessionBL : UserApi, ISession
    {
        public async Task<URegisterResp> UserRegister(URegisterData data)
        {
            return await UserRegisterAction(data);
        }

        public async Task<HttpCookie> GenCookie(string loginCredential)
        {
            return await Cookie(loginCredential);
        }

        public UserMinimal GetUserByCookie(string apiCookieValue)
        {
            return UserCookie(apiCookieValue);
        }

        public async Task<ULoginResp> UserLogin(ULoginData data)
        {
            return await UserLoginAction(data);
        }

        public async Task<UserSummary> GetUserById(int userId)
        {
            return await GetUserByIdAction(userId);
        }
    }
}
