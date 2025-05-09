using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Enums;
using eUseControl.Web.Models.User;

namespace eUseControl.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ISession _session;

        public RegisterController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister register)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserRegister, URegisterData>();
                });

                var mapper = config.CreateMapper();
                var data = mapper.Map<URegisterData>(register);

                data.RegistrationIp = Request.UserHostAddress;
                data.RegistrationDateTime = DateTime.Now;
                data.Level = URole.User;

                var userRegister = _session.UserRegister(data);

                if (userRegister.Status)
                {
                    Session["Username"] = register.Username;

                    HttpCookie cookie = _session.GenCookie(register.Username);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    TempData["SuccessMessage"] = userRegister.StatusMsg;
                    return RedirectToAction("Register", "Register", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", userRegister.StatusMsg);
                    TempData["ErrorMessage"] = userRegister.StatusMsg;

                    return RedirectToAction("Register", "Register", new { error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The model you submitted is invalid!";
                return RedirectToAction("Register", "Register", new { error = true });
            }
        } 
    }
}