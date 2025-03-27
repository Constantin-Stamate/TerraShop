using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult GeneralProfile()
        {
            return View();
        }

        // GET: ArticlesProfile
        public ActionResult ArticlesProfile()
        {
            return View();
        }

        // GET: ChangePasswordProfile
        public ActionResult ChangePasswordProfile()
        {
            return View();
        }

        // GET: PurchaseHistoryProfile
        public ActionResult PurchaseHistoryProfile()
        {
            return View();
        }

        // GET: SalesProfile
        public ActionResult SalesProfile()
        {
            return View();
        }


        // GET: SettingsProfile
        public ActionResult SettingsProfile()
        {
            return View();
        }
    }
}