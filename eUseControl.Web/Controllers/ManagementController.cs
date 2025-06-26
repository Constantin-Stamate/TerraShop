using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ManagementController : Controller
    {
        [HttpGet]
        public ActionResult UsersManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProductsManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReviewsManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CouponsManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCoupon()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrdersManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CategoriesManagement()
        {
            return View();
        }
    }
}