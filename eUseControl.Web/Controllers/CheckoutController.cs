using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
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
        private readonly IProduct _product;
        private readonly IMapper _mapper;

        public CheckoutController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _cart = bl.GetCartBL();
            _session = bl.GetSessionBL();
            _order = bl.GetOrderBL();
            _product = bl.GetProductBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Checkout(string couponCode)
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

            var allCartItems = await _cart.GetCartItemsByUserId(user.Id);

            var cartItems = _mapper.Map<List<CartCompact>>(allCartItems);

            var totals = _cart.CalculateCartTotal(allCartItems);

            decimal finalPrice = totals.totalPrice;
            decimal discountRate = 0;

            if (!string.IsNullOrEmpty(couponCode))
            {
                finalPrice = await _cart.ApplyCouponDiscount(finalPrice, couponCode);
                discountRate = _cart.ComputeDiscountAmount(totals.totalPrice, finalPrice);
            }

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
        public async Task<ActionResult> PlaceOrder(OrderViewModel model)
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

                var allCartItems = await _cart.GetCartItemsByUserId(user.Id);

                var totals = _cart.CalculateCartTotal(allCartItems);

                decimal finalPrice = totals.totalPrice;

                if (!string.IsNullOrEmpty(model.CouponCode))
                {
                    finalPrice = await _cart.ApplyCouponDiscount(finalPrice, model.CouponCode);
                }

                var orderData = _mapper.Map<OrderData>(model.Order);

                orderData.CouponCode = model.CouponCode;
                orderData.TotalPrice = _cart.ComputeOrderTotal(finalPrice, totals.shippingCost);

                var resultOrders = await _order.CancelUnpaidOrders(user.Id);
                var result = await _order.PlaceOrder(orderData, user.Id);

                if (result.Status)
                {
                    if (orderData.PaymentMethod == "Cash")
                    {
                        var updateResult = await _product.UpdateProductQuantitiesAfterOrder(allCartItems);
                        if (!updateResult.Status)
                        {
                            return RedirectToAction("Checkout", "Checkout", new { error = true });
                        }

                        var clearResult = await _cart.ClearCartItemsAfterOrder(user.Id);
                        if (!clearResult.Status)
                        {
                            return RedirectToAction("Checkout", "Checkout", new { error = true });
                        }

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