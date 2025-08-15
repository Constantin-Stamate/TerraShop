using System.Threading.Tasks;
using System.Web;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ISession
    {
        Task<URegisterResp> UserRegister(URegisterData data);

        Task<HttpCookie> GenCookie(string loginCredential);

        UserMinimal GetUserByCookie(string apiCookieValue);

        Task<ULoginResp> UserLogin(ULoginData data);

        Task<UserSummary> GetUserById(int userId);
    }
}
