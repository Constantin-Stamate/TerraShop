using System;
using System.Linq;
using System.Web.Mvc;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Extension;

namespace eUseControl.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly ISession _session;

        public BaseController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
        }

        public void SessionStatus()
        {
            var apiCookie = Request.Cookies["X-KEY"];
            if (apiCookie != null)
            {
                var profile = _session.GetUserByCookie(apiCookie.Value);
                if (profile != null)
                {
                    System.Web.HttpContext.Current.SetMySessionObject(profile);
                    System.Web.HttpContext.Current.Session["LoginStatus"] = "login";
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Clear();
                    if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("X-KEY"))
                    {
                        var cookie = ControllerContext.HttpContext.Request.Cookies["X-KEY"];
                        if (cookie != null)
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                        }
                    }

                    System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            SessionStatus();

            var currentController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var currentAction = filterContext.ActionDescriptor.ActionName;

            bool isHomePage = currentController.Equals("Main", StringComparison.OrdinalIgnoreCase) &&
                              currentAction.Equals("Index", StringComparison.OrdinalIgnoreCase);

            bool isLoginPage = currentController.Equals("Login", StringComparison.OrdinalIgnoreCase);

            bool isNavbar = currentController.Equals("Main", StringComparison.OrdinalIgnoreCase) &&
                            currentAction.Equals("Navbar", StringComparison.OrdinalIgnoreCase);

            if (!isHomePage && !isLoginPage && !isNavbar)
            {
                var loginStatus = System.Web.HttpContext.Current.Session["LoginStatus"];
                if (loginStatus == null || loginStatus.ToString() != "login")
                {
                    filterContext.Result = new RedirectResult("/Login/Login");
                }
            }
        }
    }
}