using System;
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
        public ActionResult Checkout(string couponCode)
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

            var totals = _cart.CalculateCartTotal(allCartItems);
            decimal finalPrice = totals.totalPrice;
            decimal discountRate = 0;

            if (!string.IsNullOrEmpty(couponCode))
            {
                finalPrice = _cart.ApplyCouponDiscount(finalPrice, couponCode);
                discountRate = _cart.ComputeDiscountAmount(totals.totalPrice, finalPrice);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CartData, CartCompact>(); 
            });

            var mapper = config.CreateMapper();
            var cartItems = mapper.Map<List<CartCompact>>(allCartItems);

            var model = new OrderViewModel
            {
                CartItems = cartItems,
                TotalPrice = Math.Round(_cart.ComputeOrderTotal(finalPrice, totals.shippingCost), 2),
                DiscountRate = Math.Round(discountRate, 2),
                InitialPrice = Math.Round(totals.totalPrice, 2),
                DeliveryPrice = Math.Round(totals.shippingCost, 2),
                CouponCode = couponCode
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(OrderViewModel model)
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

                var allCartItems = _cart.GetCartItemsByUserId(user.Id);

                var totals = _cart.CalculateCartTotal(allCartItems);
                decimal finalPrice = totals.totalPrice;

                if (!string.IsNullOrEmpty(model.CouponCode))
                {
                    finalPrice = _cart.ApplyCouponDiscount(finalPrice, model.CouponCode);
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<OrderCompact, OrderData>();
                });

                var mapper = config.CreateMapper();
                var orderData = mapper.Map<OrderData>(model.Order);

                orderData.CouponCode = model.CouponCode;
                orderData.TotalPrice = _cart.ComputeOrderTotal(finalPrice, totals.shippingCost);

                var resultOrders = _order.CancelUnpaidOrders(user.Id);
                var result = _order.PlaceOrder(orderData, user.Id);

                if (result.Status)
                {
                    if (orderData.PaymentMethod == "Cash")
                    {
                        var clearResult = _cart.ClearCartItemsAfterOrder(user.Id);
                        return RedirectToAction("OrderConfirmation", "Order", new { success = true, orderId = result.Id });
                    }
                    else if (orderData.PaymentMethod == "Card")
                    {
                        return RedirectToAction("Payment", "Payment", new { success = true, orderId = result.Id });
                    }

                    return RedirectToAction("Checkout", "Checkout", new { error = true });
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