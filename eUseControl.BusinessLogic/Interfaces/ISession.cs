using eUseControl.Domain.Entities;
using System.Web;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ISession
    {
        URegisterResp UserRegister(URegisterData data);

        HttpCookie GenCookie(string loginCredential);

        UserMinimal GetUserByCookie(string apiCookieValue);

        ULoginResp UserLogin(ULoginData data);
    }
}
