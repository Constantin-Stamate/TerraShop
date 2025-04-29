using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public ActionResult Payment()
        {
            return View();
        }
    }
}