﻿using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly ISession _session;
        private readonly ICart _cart;

        public CartController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _cart = bl.GetCartBL();
        }

        [HttpGet]
        public ActionResult Cart()
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

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductToCart(int productId)
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

            var result = _cart.AddItemToCart(productId, user.Id);

            if (result.Status)
            {
                return RedirectToAction("Cart", "Cart", new { success = true });
            }
            else
            {
                return RedirectToAction("Cart", "Cart", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveProductFromCart(int productId)
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

            var result = _cart.RemoveItemFromCart(productId, user.Id);

            if (result.Status)
            {
                return RedirectToAction("Cart", "Cart", new { success = true });
            }
            else
            {
                return RedirectToAction("Cart", "Cart", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProductQuantity(int productId, int newQuantity)
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

            var result = _cart.ChangeProductQuantity(productId, user.Id, newQuantity);

            if (result.Status)
            {
                return RedirectToAction("Cart", "Cart", new { success = true, pid = productId });
            }
            else
            {
                return RedirectToAction("Cart", "Cart", new { error = true, pid = productId });
            }
        }
    }
}