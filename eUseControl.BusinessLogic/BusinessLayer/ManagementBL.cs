using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ManagementBL : AdminApi, IManagement
    {
        public async Task<List<UserLite>> GetAllUsers()
        {
            return await GetAllUsersAction();
        }

        public async Task<CouponResp> AddDiscountCoupon(CouponData couponData)
        {
            return await AddDiscountCouponAction(couponData);
        }

        public async Task<List<CategoryData>> GetAllCategories()
        {
            return await GetAllCategoriesAction();
        }

        public async Task<CategoryResp> RemoveCategory(int categoryId)
        {
            return await RemoveCategoryAction(categoryId);
        }

        public async Task<CategoryResp> CreateCategory(string categoryName)
        {
            return await CreateCategoryAction(categoryName);
        }

        public async Task<List<CouponData>> GetAllDiscountCoupons()
        {
            return await GetAllDiscountCouponsAction();
        }

        public async Task<CouponResp> RemoveDiscountCoupon(int couponId)
        {
            return await RemoveDiscountCouponAction(couponId);
        }

        public async Task<List<ReviewSummary>> RetrieveAllReviews()
        {
            return await RetrieveAllReviewsAction();
        }

        public async Task<ReviewResp> RemoveReview(int reviewId)
        {
            return await RemoveReviewAction(reviewId);
        }

        public async Task<List<ProductLite>> RetrieveAllProducts()
        {
            return await RetrieveAllProductsAction();
        }

        public async Task<ProductResp> RemoveProduct(int productId)
        {
            return await RemoveProductAction(productId);
        }

        public async Task<ProductResp> ChangeRecommendationStatus(int productId)
        {
            return await ChangeRecommendationStatusAction(productId);
        }

        public async Task<List<OrderLite>> RetrieveAllOrders()
        {
            return await RetrieveAllOrdersAction();
        }

        public async Task<OrderResp> RemoveOrder(int orderId)
        {
            return await RemoveOrderAction(orderId);
        }

        public async Task<OrderResp> ChangeOrderStatus(int orderId)
        {
            return await ChangeOrderStatusAction(orderId);
        }

        public async Task<List<ContactSummary>> RetrieveAllRequests()
        {
            return await RetrieveAllRequestsAction();
        }

        public async Task<ContactResp> RemoveRequest(int contactId)
        {
            return await RemoveRequestAction(contactId);
        }

        public async Task<ContactResp> ChangeRequestStatus(int requestId)
        {
            return await ChangeRequestStatusAction(requestId);
        }
    }
}
