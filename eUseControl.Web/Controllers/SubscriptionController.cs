using System.Threading.Tasks;
using System.Web.Mvc;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.Web.Controllers
{
    public class SubscriptionController : BaseController
    {
        private readonly ISubscription _subscription;

        public SubscriptionController()
        {
            var bl = new BusinessLogicManager();
            _subscription = bl.GetSubscriptionBL();
        }

        [HttpGet]
        public ActionResult SubscriptionConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubscriber(string email)
        {
            var result = await _subscription.CreateSubscription(email);

            if (result.Status)
            {
                return RedirectToAction("SubscriptionConfirmation", "Subscription", new { success = true });
            }
            else
            {
                return RedirectToAction("Index", "Main", new { error = true });
            }
        }
    }
}