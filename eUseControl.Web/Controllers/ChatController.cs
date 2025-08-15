using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Chat;

namespace eUseControl.Web.Controllers
{
    public class ChatController : BaseController
    {
        private readonly IChat _chat;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public ChatController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _chat = bl.GetChatBL();
            _session = bl.GetSessionBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Chat()
        {
            var cookie = Request.Cookies["X-KEY"]?.Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var chats = await _chat.RetrieveUserChats(user.Id);

            var allChats = _mapper.Map<List<ChatCompact>>(chats);

            return View(allChats);
        }

        [HttpPost]
        public async Task<JsonResult> Respond(string message)
        {
            var cookie = Request.Cookies["X-KEY"]?.Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return Json(
                    new
                    {
                        success = false,
                        error = "Session expired!"
                    });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return Json(
                    new
                    {
                        success = false,
                        error = "Invalid user. Please log in again!"
                    });
            }

            var response = await _chat.GetResponse(message, user.Id);

            if (!response.Status)
            {
                return Json(
                    new
                    {
                        success = response.Status,
                        error = response.StatusMsg
                    });
            }

            return Json(
                new
                {
                    success = true,
                    responseText = response.StatusMsg
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteChatHistory()
        {
            var cookie = Request.Cookies["X-KEY"]?.Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var result = await _chat.DeleteChatHistory(user.Id);

            if (result.Status)
            {
                return RedirectToAction("Chat", "Chat", new { success = true });
            }
            else
            {
                return RedirectToAction("Chat", "Chat", new { error = true });
            }
        }
    }
}