using System;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ManagementTests
    {
        private readonly IManagement _management;

        public ManagementTests()
        {
            var bl = new BusinessLogicManager();
            _management = bl.GetManagementBL();
        }

        [TestMethod]
        public void GetAllUsersCountCheck()
        {
            var result = _management.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);
        }

        [TestMethod]
        public void AddCouponValid()
        {
            var couponData = new CouponData
            {
                Code = "TESTCOUPON",
                DiscountPercent = 10,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive = true
            };

            var result = _management.AddDiscountCoupon(couponData);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Coupon successfully added!", result.StatusMsg);
        }

        [TestMethod]
        public void AddCouponEmptyCode()
        {
            var couponData = new CouponData
            {
                Code = "",
                DiscountPercent = 10,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive = true
            };

            var result = _management.AddDiscountCoupon(couponData);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Coupon code must not be empty!", result.StatusMsg);
        }

        [TestMethod]
        public void GetCategoriesReturnsList()
        {
            var result = _management.GetAllCategories();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveCategoryNotFound()
        {
            int nonExistingCategoryId = -1;

            var result = _management.RemoveCategory(nonExistingCategoryId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Category not found!", result.StatusMsg);
        }

        [TestMethod]
        public void CreateCategoryEmptyName()
        {
            string categoryName = "";

            var result = _management.CreateCategory(categoryName);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Category name cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public void CreateCategoryValidNameSuccess()
        {
            string newCategoryName = "NewCategoryTest";

            var result = _management.CreateCategory(newCategoryName);
             
            Assert.IsTrue(result.Status);
            Assert.AreEqual("Category added successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void GetAllCouponsSuccess()
        {
            var result = _management.GetAllDiscountCoupons();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveCouponInvalid()
        {
            int invalidCouponId = -1;

            var response = _management.RemoveDiscountCoupon(invalidCouponId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Discount coupon not found!", response.StatusMsg);
        }

        [TestMethod]
        public void RetrieveReviewsSuccess()
        {
            var result = _management.RetrieveAllReviews();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveReviewNotFound()
        {
            int nonExistentReviewId = -1; 

            var response = _management.RemoveReview(nonExistentReviewId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Review not found!", response.StatusMsg);
        }

        [TestMethod]
        public void RetrieveAllProductsSuccess()
        {
            var result = _management.RetrieveAllProducts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 0);
        }

        [TestMethod]
        public void RemoveProductNotFound()
        {
            int invalidProductId = -1; 

            var response = _management.RemoveProduct(invalidProductId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Product not found!", response.StatusMsg);
        }

        [TestMethod]
        public void ChangeRecommendationSuccess()
        {
            int existingProductId = 4;

            var response = _management.ChangeRecommendationStatus(existingProductId);

            Assert.IsTrue(response.Status);
            Assert.AreEqual("Recommendation status updated successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void ChangeRecommendationNotFound()
        {
            int invalidProductId = -1;

            var response = _management.ChangeRecommendationStatus(invalidProductId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Product not found!", response.StatusMsg);
        }

        [TestMethod]
        public void RetrieveAllOrdersSuccess()
        {
            var result = _management.RetrieveAllOrders();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveOrderNotFound()
        {
            int invalidOrderId = -1; 

            var response = _management.RemoveOrder(invalidOrderId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Order not found!", response.StatusMsg);
        }

        [TestMethod]
        public void ChangeOrderStatusSuccess()
        {
            int existingOrderId = 15; 

            var response = _management.ChangeOrderStatus(existingOrderId);

            Assert.IsTrue(response.Status);
            Assert.AreEqual("Order status updated successfully!", response.StatusMsg);
        }

        [TestMethod]
        public void ChangeOrderStatusNotFound()
        {
            int invalidOrderId = -1; 

            var response = _management.ChangeOrderStatus(invalidOrderId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Order not found!", response.StatusMsg);
        }

        [TestMethod]
        public void ChangeOrderStatusAlreadyCancelled()
        {
            int cancelledOrderId = 1; 

            var response = _management.ChangeOrderStatus(cancelledOrderId);

            Assert.IsFalse(response.Status);
            Assert.AreEqual("Cannot change status of a cancelled order!", response.StatusMsg);
        }
    }
}
