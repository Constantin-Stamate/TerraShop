using System.Collections.Generic;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IManagement
    {
        List<UserLite> GetAllUsers();

        CouponResp AddDiscountCoupon(CouponData couponData);

        List<CouponData> GetAllDiscountCoupons();

        CouponResp RemoveDiscountCoupon(int couponId);

        List<CategoryData> GetAllCategories();

        CategoryResp RemoveCategory(int categoryId);

        CategoryResp CreateCategory(string categoryName);

        List<ReviewSummary> RetrieveAllReviews();

        ReviewResp RemoveReview(int reviewId);

        List<ProductLite> RetrieveAllProducts();

        ProductResp RemoveProduct(int productId);

        ProductResp ChangeRecommendationStatus(int productId);

        List<OrderLite> RetrieveAllOrders();

        OrderResp RemoveOrder(int orderId);

        OrderResp ChangeOrderStatus(int orderId);
    }
}
