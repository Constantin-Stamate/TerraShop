using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult OrderConfirmation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrderFailure() 
        {
            return View();
        }
    }
}