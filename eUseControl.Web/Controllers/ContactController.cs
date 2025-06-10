using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Web.Models.Contact;

namespace eUseControl.Web.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContact _contact;
        private readonly ISession _session;

        public ContactController()
        {
            var bl = new BusinessLogicManager();
            _contact = bl.GetContactBL();
            _session = bl.GetSessionBL();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitContactRequest(ContactCompact contactCompact)
        {
            if (ModelState.IsValid)
            {
                var cookie = Request.Cookies["X-KEY"]?.Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ContactCompact, ContactData>();
                });

                var mapper = config.CreateMapper();
                var contactData = mapper.Map<ContactData>(contactCompact);

                var result = _contact.SubmitContactRequest(contactData, user.Id);

                if (result.Status)
                {
                    return RedirectToAction("ThankYou", "Main", new { success = true });
                }
                else
                {
                    return RedirectToAction("Contact", "Contact", new { error = true });
                }
            }
            else
            {
                return RedirectToAction("Contact", "Contact", new { error = true });
            }
        }
    }
}