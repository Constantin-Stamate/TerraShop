using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class LegalController : Controller
    {
        [HttpGet]
        public ActionResult PrivacyPolicy() 
        {
            return View();
        }

        [HttpGet]
        public ActionResult SalesAndRefunds()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TermsOfUse()
        {
            return View();
        }
    }
}