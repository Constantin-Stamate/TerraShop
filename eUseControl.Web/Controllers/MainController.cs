using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        // GET: Error
        public ActionResult Error404()
        {
            return View();
        }

        // GET: ThankYou
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}