using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic;
using AutoMapper;
using eUseControl.Domain.Entities.Product;
using System.Collections.Generic;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly IProduct _product;
        private readonly IWishlist _wishlist;
        private readonly ISession _session;
        private readonly ICart _cart;

        public MainController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _wishlist = bl.GetWishlistBL();
            _session = bl.GetSessionBL();
            _cart = bl.GetCartBL();
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Navbar()
        {
            var cookie = Request.Cookies["X-KEY"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return PartialView("_Navbar", new ProductNavigationViewModel
                {
                    Categories = new Dictionary<ProductCategory, int>(),
                    WishlistCount = 0
                });
            }

            var user = _session.GetUserByCookie(cookie.Value);
            if (user == null)
            {
                return PartialView("_Navbar", new ProductNavigationViewModel
                {
                    Categories = new Dictionary<ProductCategory, int>(),
                    WishlistCount = 0
                });
            }

            var categoryProductCounts = _product.GetCategoryProductCounts();
            var wishlistProductsCount = _wishlist.GetWishlistCountByUserId(user.Id);
            var cartProductsCount = _cart.GetCartCountByUserId(user.Id); 

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductSummary, ProductMini>();
                cfg.CreateMap<CategoryDbTable, ProductCategory>();
            });

            var mapper = config.CreateMapper();
            var productCountsByCategory = mapper.Map<Dictionary<ProductCategory, int>>(categoryProductCounts);

            var model = new ProductNavigationViewModel
            {
                Categories = productCountsByCategory,
                WishlistCount = wishlistProductsCount,
                CartCount = cartProductsCount
            };

            return PartialView("_Navbar", model);
        }

        [HttpGet]
        public ActionResult Error404(bool? error)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}