using System.Web.Mvc;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic;
using AutoMapper;
using eUseControl.Domain.Entities.Product;
using System.Collections.Generic;
using eUseControl.Web.Models.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Web.Models.Profile;
using eUseControl.Domain.Entities.Review;
using eUseControl.Web.Models.Review;
using eUseControl.Web.Models.Main;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly IProduct _product;
        private readonly IWishlist _wishlist;
        private readonly ISession _session;
        private readonly ICart _cart;
        private readonly IReview _review;

        public MainController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _wishlist = bl.GetWishlistBL();
            _session = bl.GetSessionBL();
            _cart = bl.GetCartBL();
            _review = bl.GetReviewBL();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var allReviews = _review.RetrieveAllReviews();

            var list = new List<ReviewProfileData>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProfileData, ProfileMini>();
                cfg.CreateMap<ReviewData, ReviewMini>();
            });

            var mapper = config.CreateMapper();

            foreach (var pair in allReviews)
            {
                var review = mapper.Map<ReviewMini>(pair.Key);
                var profile = mapper.Map<ProfileMini>(pair.Value);

                list.Add(new ReviewProfileData
                {
                    Review = review,
                    Profile = profile
                });
            }

            var model = new MainViewModel
            {
                ReviewsWithProfiles = list
            };

            return View(model);
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