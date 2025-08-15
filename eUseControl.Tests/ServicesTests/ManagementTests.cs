using System;
using System.Threading.Tasks;
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
        public async Task GetAllUsersCountCheck()
        {
            var result = await _management.GetAllUsers();

            Assert.IsNotNull(result, "Expected a non-null list of users!");
            Assert.IsTrue(result.Count >= 0, "Expected user count to be zero or more!");
        }

        [TestMethod]
        public async Task AddCouponValid()
        {
            var couponData = new CouponData
            {
                Code = "TESTCOUPON10",
                DiscountPercent = 10,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive = true
            };

            var result = await _management.AddDiscountCoupon(couponData);

            Assert.IsTrue(result.Status, "Expected the coupon to be added successfully!");
            Assert.AreEqual("Coupon successfully added!", result.StatusMsg);
        }

        [TestMethod]
        public async Task AddCouponEmptyCode()
        {
            var couponData = new CouponData
            {
                Code = "",
                DiscountPercent = 10,
                ExpirationDate = DateTime.Now.AddDays(10),
                IsActive = true
            };

            var result = await _management.AddDiscountCoupon(couponData);

            Assert.IsFalse(result.Status, "Expected failure due to empty coupon code!");
            Assert.AreEqual("Coupon code must not be empty!", result.StatusMsg);
        }

        [TestMethod]
        public async Task GetCategoriesReturnsList()
        {
            var result = await _management.GetAllCategories();

            Assert.IsNotNull(result, "Expected a non-null list of categories!");
            Assert.IsTrue(result.Count > 0, "Expected at least one category to be returned!");
        }

        [TestMethod]
        public async Task RemoveCategoryNotFound()
        {
            int nonExistingCategoryId = -1;

            var result = await _management.RemoveCategory(nonExistingCategoryId);

            Assert.IsFalse(result.Status, "Expected failure because the category does not exist!");
            Assert.AreEqual("Category not found!", result.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveCategorySuccess()
        {
            int existingCategoryId = 3;

            var result = await _management.RemoveCategory(existingCategoryId);

            Assert.IsTrue(result.Status, "Expected success because the category exists and should be deleted!");
            Assert.AreEqual("Category successfully deleted!", result.StatusMsg);
        }

        [TestMethod]
        public async Task CreateCategoryEmptyName()
        {
            string categoryName = "";

            var result = await _management.CreateCategory(categoryName);

            Assert.IsFalse(result.Status, "Expected failure due to empty category name!");
            Assert.AreEqual("Category name cannot be empty!", result.StatusMsg);
        }

        [TestMethod]
        public async Task CreateCategoryValidNameSuccess()
        {
            string newCategoryName = "NewCategory";

            var result = await _management.CreateCategory(newCategoryName);

            Assert.IsTrue(result.Status, "Expected successful creation of category!");
            Assert.AreEqual("Category added successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task GetAllCouponsSuccess()
        {
            var result = await _management.GetAllDiscountCoupons();

            Assert.IsNotNull(result, "Expected a non-null list of discount coupons!");
            Assert.IsTrue(result.Count > 0, "Expected at least one coupon to be returned!");
        }

        [TestMethod]
        public async Task RemoveCouponInvalid()
        {
            int invalidCouponId = -1;

            var response = await _management.RemoveDiscountCoupon(invalidCouponId);

            Assert.IsFalse(response.Status, "Expected failure because the coupon does not exist!");
            Assert.AreEqual("Discount coupon not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveDiscountCouponSuccess()
        {
            int existingCouponId = 3;

            var response = await _management.RemoveDiscountCoupon(existingCouponId);

            Assert.IsTrue(response.Status, "Expected success because the discount coupon exists and should be deleted!");
            Assert.AreEqual("Discount coupon successfully deleted!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RetrieveReviewsSuccess()
        {
            var result = await _management.RetrieveAllReviews();

            Assert.IsNotNull(result, "Expected a non-null list of reviews!");
            Assert.IsTrue(result.Count > 0, "Expected at least one review to be returned!");
        }

        [TestMethod]
        public async Task RemoveReviewNotFound()
        {
            int nonExistentReviewId = -1;

            var response = await _management.RemoveReview(nonExistentReviewId);

            Assert.IsFalse(response.Status, "Expected failure because the review does not exist!");
            Assert.AreEqual("Review not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveReviewSuccess()
        {
            int existingReviewId = 3;

            var response = await _management.RemoveReview(existingReviewId);

            Assert.IsTrue(response.Status, "Expected success because the review exists and should be deleted!");
            Assert.AreEqual("Review successfully deleted!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RetrieveAllProductsSuccess()
        {
            var result = await _management.RetrieveAllProducts();

            Assert.IsNotNull(result, "Expected a non-null list of products!");
            Assert.IsTrue(result.Count >= 0, "Expected product count to be zero or more!");
        }

        [TestMethod]
        public async Task RemoveProductNotFound()
        {
            int invalidProductId = -1;

            var response = await _management.RemoveProduct(invalidProductId);

            Assert.IsFalse(response.Status, "Expected failure because the product does not exist!");
            Assert.AreEqual("Product not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveProductSuccess()
        {
            int existingProductId = 3;

            var response = await _management.RemoveProduct(existingProductId);

            Assert.IsTrue(response.Status, "Expected success because the product exists and should be deleted!");
            Assert.AreEqual("Product successfully deleted!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeRecommendationSuccess()
        {
            int existingProductId = 4;

            var response = await _management.ChangeRecommendationStatus(existingProductId);

            Assert.IsTrue(response.Status, "Expected recommendation status to be updated successfully!");
            Assert.AreEqual("Recommendation status updated successfully!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeRecommendationNotFound()
        {
            int invalidProductId = -1;

            var response = await _management.ChangeRecommendationStatus(invalidProductId);

            Assert.IsFalse(response.Status, "Expected failure because the product does not exist!");
            Assert.AreEqual("Product not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RetrieveAllOrdersSuccess()
        {
            var result = await _management.RetrieveAllOrders();

            Assert.IsNotNull(result, "Expected a non-null list of orders!");
            Assert.IsTrue(result.Count > 0, "Expected at least one order to be returned!");
        }

        [TestMethod]
        public async Task RemoveOrderNotFound()
        {
            int invalidOrderId = -1;

            var response = await _management.RemoveOrder(invalidOrderId);

            Assert.IsFalse(response.Status, "Expected failure because the order does not exist!");
            Assert.AreEqual("Order not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveOrderSuccess()
        {
            int existingOrderId = 3;

            var response = await _management.RemoveOrder(existingOrderId);

            Assert.IsTrue(response.Status, "Expected success because the order exists and should be deleted!");
            Assert.AreEqual("Order successfully deleted!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeOrderStatusSuccess()
        {
            int existingOrderId = 9;

            var response = await _management.ChangeOrderStatus(existingOrderId);

            Assert.IsTrue(response.Status, "Expected the order status to be updated successfully!");
            Assert.AreEqual("Order status updated successfully!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeOrderStatusNotFound()
        {
            int invalidOrderId = -1;

            var response = await _management.ChangeOrderStatus(invalidOrderId);

            Assert.IsFalse(response.Status, "Expected failure because the order does not exist!");
            Assert.AreEqual("Order not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeOrderStatusAlreadyCancelled()
        {
            int cancelledOrderId = 1;

            var response = await _management.ChangeOrderStatus(cancelledOrderId);

            Assert.IsFalse(response.Status, "Expected failure because the order is already cancelled!");
            Assert.AreEqual("Cannot change status of a cancelled order!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RetrieveAllRequestsSuccess()
        {
            var result = await _management.RetrieveAllRequests();

            Assert.IsNotNull(result, "Expected a non-null list of contact requests!");
            Assert.IsTrue(result.Count >= 0, "Expected contact requests count to be zero or more!");
        }

        [TestMethod]
        public async Task RemoveRequestSuccess()
        {
            int existingRequestId = 3;

            var response = await _management.RemoveRequest(existingRequestId);

            Assert.IsTrue(response.Status, "Expected successful deletion of the contact request!");
            Assert.AreEqual("Contact request successfully deleted!", response.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveRequestNotFound()
        {
            int invalidRequestId = -1;

            var response = await _management.RemoveRequest(invalidRequestId);

            Assert.IsFalse(response.Status, "Expected failure because the contact request does not exist!");
            Assert.AreEqual("Contact request not found!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeRequestStatusSuccess()
        {
            int existingRequestId = 4;

            var response = await _management.ChangeRequestStatus(existingRequestId);

            Assert.IsTrue(response.Status, "Expected contact request status to be updated successfully!");
            Assert.AreEqual("Contact request status updated successfully!", response.StatusMsg);
        }

        [TestMethod]
        public async Task ChangeRequestStatusNotFound()
        {
            int invalidRequestId = -1;

            var response = await _management.ChangeRequestStatus(invalidRequestId);

            Assert.IsFalse(response.Status, "Expected failure because the contact request does not exist!");
            Assert.AreEqual("Contact request not found!", response.StatusMsg);
        }
    }
}
