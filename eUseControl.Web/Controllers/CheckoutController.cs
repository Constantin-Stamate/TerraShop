using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Web.Models.Cart;
using eUseControl.Web.Models.Order;

namespace eUseControl.Web.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly ICart _cart;
        private readonly ISession _session;
        private readonly IOrder _order;

        public CheckoutController()
        {
            var bl = new BusinessLogicManager();
            _cart = bl.GetCartBL();
            _session = bl.GetSessionBL();
            _order = bl.GetOrderBL();
        }

        [HttpGet]
        public ActionResult Checkout(decimal totalPrice)
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

            var allCartItems = _cart.GetCartItemsByUserId(user.Id);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CartData, CartCompact>(); 
            });

            var mapper = config.CreateMapper();
            var cartItems = mapper.Map<List<CartCompact>>(allCartItems);

            OrderCompact orderCompact = new OrderCompact();
            orderCompact.TotalPrice = totalPrice;

            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                Order = orderCompact
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
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

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderCompact, OrderData>();
                });

                var mapper = config.CreateMapper();
                var orderData = mapper.Map<OrderData>(model.Order);

                var result = _order.PlaceOrder(orderData, user.Id);

                if (result.Status)
                {
                    return RedirectToAction("Payment", "Payment", new { success = true });
                }
                else
                {
                    return RedirectToAction("Checkout", "Checkout", new { error = true });
                }
            }
            else
            {
                return RedirectToAction("Checkout", "Checkout", new { error = true });
            }
        }
    }
}