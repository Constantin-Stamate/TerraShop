using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class WishlistController : BaseController
    {
        private readonly IWishlist _wishlist;
        private readonly ISession _session;

        public WishlistController()
        {
            var bl = new BusinessLogicManager();
            _session = bl.GetSessionBL();
            _wishlist = bl.GetWishlistBL();
        }

        [HttpGet]
        public ActionResult Wishlist()
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

            var products = _wishlist.GetAllWishlistProducts(user.Id);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductLite, ProductInfo >();
            });

            var mapper = config.CreateMapper();
            var productsList = mapper.Map<List<ProductInfo>>(products);

            return View(productsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductToWishlist(int productId)
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

            var result = _wishlist.AddProductToWishlist(user.Id, productId);

            if (result.Status)
            {
                return RedirectToAction("Shop", "Shop", new { success = true });
            }
            else
            {
                return RedirectToAction("Shop", "Shop", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveProductFromWishlist(int productId)
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

            var result = _wishlist.RemoveProductFromWishlist(productId, user.Id);

            if (result.Status)
            {
                return RedirectToAction("Shop", "Shop", new { success = true });
            }
            else
            {
                return RedirectToAction("Shop", "Shop", new { error = true });
            }
        }
    }
}