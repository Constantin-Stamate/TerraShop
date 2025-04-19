using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }
    }
}