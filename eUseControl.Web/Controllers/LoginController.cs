using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.User;
using eUseControl.Domain.Enums;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISession _session;

        public LoginController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserLogin, ULoginData>();
                });

                var mapper = config.CreateMapper();
                var data = mapper.Map<ULoginData>(login);

                data.LastIp = Request.UserHostAddress;
                data.LastLogin = DateTime.Now;
                data.Level = URole.User;

                var userLogin = _session.UserLogin(data);

                if (userLogin.Status)
                {
                    Session["Username"] = login.Username;
                    Session["User"] = userLogin.UserMinimal;

                    HttpCookie cookie = _session.GenCookie(login.Username);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "Main", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", userLogin.StatusMsg);
                    TempData["ErrorMessage"] = userLogin.StatusMsg;

                    return RedirectToAction("Login", "Login", new { error = true });
                }
            }

            return View();
        }
    }
}