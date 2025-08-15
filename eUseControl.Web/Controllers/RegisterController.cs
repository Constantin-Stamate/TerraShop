using System;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;

        public RegisterController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserRegister register)
        {
            if (ModelState.IsValid)
            {
                var data = _mapper.Map<URegisterData>(register);

                data.RegistrationIp = Request.UserHostAddress;
                data.RegistrationDateTime = DateTime.Now;
                data.Level = URole.User;

                var userRegister = await _session.UserRegister(data);

                if (userRegister.Status)
                {
                    Session["Username"] = register.Username;

                    HttpCookie cookie = await _session.GenCookie(register.Username);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    TempData["SuccessMessage"] = userRegister.StatusMsg;
                    return RedirectToAction("Register", "Register", new { success = true });
                }
                else
                {
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