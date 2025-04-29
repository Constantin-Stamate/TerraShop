using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class CartController : BaseController
    {
        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }
    }
}