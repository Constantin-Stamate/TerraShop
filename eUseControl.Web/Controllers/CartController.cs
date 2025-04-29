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