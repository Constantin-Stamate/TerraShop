using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ManagementBL : AdminApi, IManagement
    {
        public List<UserLite> GetAllUsers()
        {
            return GetAllUsersAction();
        }

        public CouponResp AddDiscountCoupon(CouponData couponData)
        {
            return AddDiscountCouponAction(couponData);
        }

        public List<CategoryData> GetAllCategories()
        {
            return GetAllCategoriesAction();
        }

        public CategoryResp RemoveCategory(int categoryId)
        {
            return RemoveCategoryAction(categoryId);
        }

        public CategoryResp CreateCategory(string categoryName)
        {
            return CreateCategoryAction(categoryName);
        }

        public List<CouponData> GetAllDiscountCoupons()
        {
            return GetAllDiscountCouponsAction();
        }

        public CouponResp RemoveDiscountCoupon(int couponId)
        {
            return RemoveDiscountCouponAction(couponId);
        }

        public List<ReviewSummary> RetrieveAllReviews()
        {
            return RetrieveAllReviewsAction();
        }

        public ReviewResp RemoveReview(int reviewId)
        {
            return RemoveReviewAction(reviewId);
        }

        public List<ProductLite> RetrieveAllProducts()
        {
            return RetrieveAllProductsAction();
        }

        public ProductResp RemoveProduct(int productId)
        {
            return RemoveProductAction(productId);
        }

        public ProductResp ChangeRecommendationStatus(int productId)
        {
            return ChangeRecommendationStatusAction(productId);
        }

        public List<OrderLite> RetrieveAllOrders()
        {
            return RetrieveAllOrdersAction();
        }

        public OrderResp RemoveOrder(int orderId)
        {
            return RemoveOrderAction(orderId);
        }

        public OrderResp ChangeOrderStatus(int orderId)
        {
            return ChangeOrderStatusAction(orderId);
        }
    }
}
