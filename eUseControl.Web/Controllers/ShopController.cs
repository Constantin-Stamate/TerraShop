using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ShopController : BaseController
    {
        [HttpGet]
        public ActionResult Shop()
        {
            return View();
        }
    }
}