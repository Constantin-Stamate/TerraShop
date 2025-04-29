using System.Web.Mvc;

namespace eUseControl.Web.Controllers
{
    public class ContactController : BaseController
    {
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
    }
}