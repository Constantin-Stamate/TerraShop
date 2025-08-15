using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Order;

namespace eUseControl.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrder _order;
        private readonly IMapper _mapper;

        public OrderController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _order = bl.GetOrderBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> OrderConfirmation(int orderId)
        {
            var orderData = await _order.GetOrderById(orderId);

            var order = _mapper.Map<OrderMini>(orderData);

            return View(order);
        }

        [HttpGet]
        public async Task<ActionResult> OrderFailure(int orderId)
        {
            var orderData = await _order.GetOrderById(orderId);

            var order = _mapper.Map<OrderMini>(orderData);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var result = await _order.CancelOrder(orderId);

            if (result.Status)
            {
                return RedirectToAction("OrdersProfile", "Profile", new { success = true });
            }
            else
            {
                return RedirectToAction("OrdersProfile", "Profile", new { error = true });
            }
        }
    }
}