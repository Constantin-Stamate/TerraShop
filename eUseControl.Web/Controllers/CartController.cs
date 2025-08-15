using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly ISession _session;
        private readonly ICart _cart;
        private readonly IMapper _mapper;

        public CartController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _cart = bl.GetCartBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Cart()
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

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductToCart(int productId)
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

            var result = await _cart.AddItemToCart(productId, user.Id);

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
        public async Task<ActionResult> RemoveProductFromCart(int productId)
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

            var result = await _cart.RemoveItemFromCart(productId, user.Id);

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
        public async Task<ActionResult> UpdateProductQuantity(int productId, int newQuantity)
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

            var result = await _cart.ChangeProductQuantity(productId, user.Id, newQuantity);

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