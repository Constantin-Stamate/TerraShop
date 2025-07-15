using System.Web.Mvc;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChat _chat;

        public ChatController()
        {
            var BL = new BusinessLogicManager();
            _chat = BL.GetChatBL();
        }

        [HttpGet]
        public ActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Respond(string message)
        {
            var response = _chat.GetResponse(message);

            return Json(new { responseText = response });
        }
    }
}