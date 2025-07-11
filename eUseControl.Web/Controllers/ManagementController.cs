using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;
using eUseControl.Web.Filtres;
using eUseControl.Web.Models.Cart;
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

        public ManagementController()
        {
            var bl = new BusinessLogicManager();
            _management = bl.GetManagementBL();
        }

        [HttpGet]
        public ActionResult UsersManagement()
        {
            var users = _management.GetAllUsers();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserLite, UserInfo>();
            });

            var mapper = config.CreateMapper();
            var allUsers = mapper.Map<List<UserInfo>>(users);

            return View(allUsers);
        }

        [HttpGet]
        public ActionResult ProductsManagement()
        {
            var products = _management.RetrieveAllProducts();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductLite, ProductInfo>();
            });

            var mapper = config.CreateMapper();
            var allProducts = mapper.Map<List<ProductInfo>>(products);

            return View(allProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int productId)
        {
            var result = _management.RemoveProduct(productId);

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
        public ActionResult ChangeRecommendationStatus(int productId)
        {
            var result = _management.ChangeRecommendationStatus(productId);

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
        public ActionResult ReviewsManagement()
        {
            var reviews = _management.RetrieveAllReviews();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewSummary, ReviewInfo>();
            });

            var mapper = config.CreateMapper();
            var allReviews = mapper.Map<List<ReviewInfo>>(reviews);

            return View(allReviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReview(int reviewId)
        {
            var result = _management.RemoveReview(reviewId);

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
        public ActionResult CouponsManagement()
        {
            var discountCoupons = _management.GetAllDiscountCoupons();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CouponData, CouponCompact>();
            });

            var mapper = config.CreateMapper();
            var allDiscountCoupons = mapper.Map<List<CouponCompact>>(discountCoupons);

            return View(allDiscountCoupons);
        }

        [HttpGet]
        public ActionResult AddCoupon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCoupon(CouponCompact couponCompact)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CouponCompact, CouponData>();
                });

                var mapper = config.CreateMapper();
                var couponData = mapper.Map<CouponData>(couponCompact); 

                var result = _management.AddDiscountCoupon(couponData);

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
        public ActionResult DeleteDiscountCoupon(int couponId)
        {
            var result = _management.RemoveDiscountCoupon(couponId);

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
        public ActionResult OrdersManagement()
        {
            var orders = _management.RetrieveAllOrders();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderLite, OrderInfo>();
            });

            var mapper = config.CreateMapper();
            var allOrders = mapper.Map<List<OrderInfo>>(orders);

            return View(allOrders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder(int orderId)
        {
            var result = _management.RemoveOrder(orderId);

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
        public ActionResult ChangeOrderStatus(int orderId)
        {
            var result = _management.ChangeOrderStatus(orderId);

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
        public ActionResult CategoriesManagement()
        {
            var categories = _management.GetAllCategories();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryData, ProductCategory>();
            });

            var mapper = config.CreateMapper();
            var allCategories = mapper.Map<List<ProductCategory>>(categories);

            return View(allCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int categoryId)
        {
            var result = _management.RemoveCategory(categoryId);

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
        public ActionResult AddCategory(string categoryName)
        {
            var result = _management.CreateCategory(categoryName);

            if (result.Status)
            {
                return RedirectToAction("CategoriesManagement", "Management", new { success = true });
            }
            else
            {
                return RedirectToAction("CategoriesManagement", "Management", new { error = true });
            }
        }
    }
}