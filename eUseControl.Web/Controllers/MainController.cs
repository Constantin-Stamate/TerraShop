using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Main;
using eUseControl.Web.Models.Product;
using eUseControl.Web.Models.Profile;
using eUseControl.Web.Models.Review;

namespace eUseControl.Web.Controllers
{
    public class MainController : BaseController
    {
        private readonly IProduct _product;
        private readonly IWishlist _wishlist;
        private readonly ISession _session;
        private readonly ICart _cart;
        private readonly IReview _review;
        private readonly IMapper _mapper;

        public MainController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _wishlist = bl.GetWishlistBL();
            _session = bl.GetSessionBL();
            _cart = bl.GetCartBL();
            _review = bl.GetReviewBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var cookieValue = Request.Cookies["X-KEY"]?.Value;

            var user = !string.IsNullOrEmpty(cookieValue) ? _session.GetUserByCookie(cookieValue) : null;

            var productIds = user != null ? await _wishlist.GetWishlistProductIds(user.Id) : new List<int>();

            var allReviews = await _review.RetrieveAllReviews();

            var reviewsWithProfiles = allReviews
                .Select(pair => new ReviewProfileData
                {
                    Review = _mapper.Map<ReviewMini>(pair.Key),
                    Profile = _mapper.Map<ProfileMini>(pair.Value)
                })
                .ToList();

            var topProducts = await _product.GetProductsFromTopCategories();

            var topProductsMapped = topProducts.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                .Select(p => _mapper.Map<ProductMini>(p))
                .ToList()
            );

            var model = new MainViewModel
            {
                ReviewsWithProfiles = reviewsWithProfiles,
                Products = topProductsMapped,
                WishlistProductIds = productIds
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
                    WishlistCount = 0,
                    CartCount = 0
                });
            }

            var user = _session.GetUserByCookie(cookie.Value);
            if (user == null)
            {
                return PartialView("_Navbar", new ProductNavigationViewModel
                {
                    Categories = new Dictionary<ProductCategory, int>(),
                    WishlistCount = 0,
                    CartCount = 0
                });
            }

            var categoryProductCounts = _product.GetCategoryProductCounts();

            var productCountsByCategory = _mapper.Map<Dictionary<ProductCategory, int>>(categoryProductCounts);

            var wishlistProductsCount = _wishlist.GetWishlistCountByUserId(user.Id);

            var cartProductsCount = _cart.GetCartCountByUserId(user.Id);

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