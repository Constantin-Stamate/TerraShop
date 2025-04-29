using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}