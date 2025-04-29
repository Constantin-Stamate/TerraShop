using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class OrderController : Controller
    {
        // GET: OrderConfirmation
        public ActionResult OrderConfirmation()
        {
            return View();
        }

        // GET: OrderFailure
        public ActionResult OrderFailure() 
        {
            return View();
        }
    }
}