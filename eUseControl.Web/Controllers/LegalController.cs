using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class LegalController : Controller
    {
        // GET: PrivacyPolicy
        public ActionResult PrivacyPolicy() 
        {
            return View();
        }

        // GET: SalesAndRefunds
        public ActionResult SalesAndRefunds()
        {
            return View();
        }

        // GET: TermsOfUse
        public ActionResult TermsOfUse()
        {
            return View();
        }
    }
}