using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Entities;
using System.Web;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface ISession
    {
        URegisterResp UserRegister(URegisterData data);

        HttpCookie GenCookie(string loginCredential);

        UserMinimal GetUserByCookie(string apiCookieValue);
    }
}
