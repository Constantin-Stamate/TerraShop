using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class CheckoutController : Controller
    {
        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
        }
    }
}