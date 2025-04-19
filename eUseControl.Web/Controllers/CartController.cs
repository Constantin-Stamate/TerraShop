using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        public ActionResult Cart()
        {
            return View();
        }
    }
}