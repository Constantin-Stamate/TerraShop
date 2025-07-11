using System.Web;
using System.Web.Mvc;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Enums;

namespace eUseControl.Web.Filtres
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.Session["User"] as UserMinimal;

            if (httpContext.Session == null || httpContext.Session["User"] == null)
            {
                return false;
            }

            return user?.Level == URole.Admin;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login/Login?error=true");
        }
    }
}