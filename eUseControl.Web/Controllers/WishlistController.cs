using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class WishlistController : BaseController
    {
        private readonly IWishlist _wishlist;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public WishlistController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _wishlist = bl.GetWishlistBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Wishlist()
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

            var products = await _wishlist.GetAllWishlistProducts(user.Id);

            var productsList = _mapper.Map<List<ProductInfo>>(products);

            return View(productsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductToWishlist(int productId)
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

            var result = await _wishlist.AddProductToWishlist(user.Id, productId);

            if (result.Status)
            {
                return RedirectToAction("Wishlist", "Wishlist", new { success = true });
            }
            else
            {
                return RedirectToAction("Wishlist", "Wishlist", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveProductFromWishlist(int productId)
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

            var result = await _wishlist.RemoveProductFromWishlist(productId, user.Id);

            if (result.Status)
            {
                return RedirectToAction("Wishlist", "Wishlist", new { success = true });
            }
            else
            {
                return RedirectToAction("Wishlist", "Wishlist", new { error = true });
            }
        }
    }
}