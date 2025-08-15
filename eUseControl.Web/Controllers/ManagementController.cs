using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Web.Filtres;
using eUseControl.Web.Models.Cart;
using eUseControl.Web.Models.Contact;
using eUseControl.Web.Models.Order;
using eUseControl.Web.Models.Product;
using eUseControl.Web.Models.Review;
using eUseControl.Web.Models.User;

namespace eUseControl.Web.Controllers
{
    [AdminAuthorize]
    public class ManagementController : BaseController
    {
        private readonly IManagement _management;
        private readonly IMapper _mapper;

        public ManagementController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _management = bl.GetManagementBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> UsersManagement()
        {
            var users = await _management.GetAllUsers();

            var allUsers = _mapper.Map<List<UserInfo>>(users);

            return View(allUsers);
        }

        [HttpGet]
        public async Task<ActionResult> ProductsManagement()
        {
            var products = await _management.RetrieveAllProducts();

            var allProducts = _mapper.Map<List<ProductInfo>>(products);

            return View(allProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var result = await _management.RemoveProduct(productId);

            if (result.Status)
            {
                return RedirectToAction("ProductsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("ProductsManagement", "Management", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRecommendationStatus(int productId)
        {
            var result = await _management.ChangeRecommendationStatus(productId);

            if (result.Status)
            {
                return RedirectToAction("ProductsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("ProductsManagement", "Management", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReviewsManagement()
        {
            var reviews = await _management.RetrieveAllReviews();

            var allReviews = _mapper.Map<List<ReviewInfo>>(reviews);

            return View(allReviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            var result = await _management.RemoveReview(reviewId);

            if (result.Status)
            {
                return RedirectToAction("ReviewsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("ReviewsManagement", "Management", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> CouponsManagement()
        {
            var discountCoupons = await _management.GetAllDiscountCoupons();

            var allDiscountCoupons = _mapper.Map<List<CouponCompact>>(discountCoupons);

            return View(allDiscountCoupons);
        }

        [HttpGet]
        public ActionResult AddCoupon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCoupon(CouponCompact couponCompact)
        {
            if (ModelState.IsValid)
            {
                var couponData = _mapper.Map<CouponData>(couponCompact);

                var result = await _management.AddDiscountCoupon(couponData);

                if (result.Status)
                {
                    return RedirectToAction("CouponsManagement", "Management", new { success = true });
                }
                else
                {
                    return RedirectToAction("AddCoupon", "Management", new { error = true });
                }
            }
            else
            {
                return RedirectToAction("AddCoupon", "Management", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDiscountCoupon(int couponId)
        {
            var result = await _management.RemoveDiscountCoupon(couponId);

            if (result.Status)
            {
                return RedirectToAction("CouponsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("CouponsManagement", "Management", new { error = true });
            }
        }
        [HttpGet]
        public async Task<ActionResult> OrdersManagement()
        {
            var orders = await _management.RetrieveAllOrders();

            var allOrders = _mapper.Map<List<OrderInfo>>(orders);

            return View(allOrders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var result = await _management.RemoveOrder(orderId);

            if (result.Status)
            {
                return RedirectToAction("OrdersManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("OrdersManagement", "Management", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeOrderStatus(int orderId)
        {
            var result = await _management.ChangeOrderStatus(orderId);

            if (result.Status)
            {
                return RedirectToAction("OrdersManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("OrdersManagement", "Management", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> CategoriesManagement()
        {
            var categories = await _management.GetAllCategories();

            var allCategories = _mapper.Map<List<ProductCategory>>(categories);

            return View(allCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var result = await _management.RemoveCategory(categoryId);

            if (result.Status)
            {
                return RedirectToAction("CategoriesManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("CategoriesManagement", "Management", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCategory(string categoryName)
        {
            var result = await _management.CreateCategory(categoryName);

            if (result.Status)
            {
                return RedirectToAction("CategoriesManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("CategoriesManagement", "Management", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> RequestsManagement()
        {
            var requests = await _management.RetrieveAllRequests();

            var allRequests = _mapper.Map<List<ContactInfo>>(requests);

            return View(allRequests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRequest(int requestId)
        {
            var result = await _management.RemoveRequest(requestId);

            if (result.Status)
            {
                return RedirectToAction("RequestsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("RequestsManagement", "Management", new { error = true });
            }
        }

        public async Task<ActionResult> ChangeRequestStatus(int requestId)
        {
            var result = await _management.ChangeRequestStatus(requestId);

            if (result.Status)
            {
                return RedirectToAction("RequestsManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("RequestsManagement", "Management", new { error = true });
            }
        }
    }
}