using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Order;
using eUseControl.Web.Models.Order;

namespace eUseControl.Web.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrder _order;
        private readonly ISession _session;

        public OrderController()
        {
            var bl = new BusinessLogicManager();
            _order = bl.GetOrderBL();
            _session = bl.GetSessionBL();
        }

        [HttpGet]
        public ActionResult OrderConfirmation(int orderId)
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

            var orderData = _order.GetOrderById(orderId);
            if (orderData == null)
            {
                return RedirectToAction("OrderConfirmation", "Order", new { error = true, orderId });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderMinimal, OrderMini>();
            });

            var mapper = config.CreateMapper();
            var order = mapper.Map<OrderMini>(orderData);

            return View(order);
        }

        [HttpGet]
        public ActionResult OrderFailure(int orderId) 
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

            var orderData = _order.GetOrderById(orderId);
            if (orderData == null)
            {
                return RedirectToAction("OrderFailure", "Order", new { error = true, orderId });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderMinimal, OrderMini>();
            });

            var mapper = config.CreateMapper();
            var order = mapper.Map<OrderMini>(orderData);

            return View(order);
        }
    }
}