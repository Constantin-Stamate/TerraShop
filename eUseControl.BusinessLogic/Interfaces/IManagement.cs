using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Contact;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Review;
using eUseControl.Domain.Entities.User;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IManagement
    {
        Task<List<UserLite>> GetAllUsers();

        Task<CouponResp> AddDiscountCoupon(CouponData couponData);

        Task<List<CouponData>> GetAllDiscountCoupons();

        Task<CouponResp> RemoveDiscountCoupon(int couponId);

        Task<List<CategoryData>> GetAllCategories();

        Task<CategoryResp> RemoveCategory(int categoryId);

        Task<CategoryResp> CreateCategory(string categoryName);

        Task<List<ReviewSummary>> RetrieveAllReviews();

        Task<ReviewResp> RemoveReview(int reviewId);

        Task<List<ProductLite>> RetrieveAllProducts();

        Task<ProductResp> RemoveProduct(int productId);

        Task<ProductResp> ChangeRecommendationStatus(int productId);

        Task<List<OrderLite>> RetrieveAllOrders();

        Task<OrderResp> RemoveOrder(int orderId);

        Task<OrderResp> ChangeOrderStatus(int orderId);

        Task<List<ContactSummary>> RetrieveAllRequests();

        Task<ContactResp> RemoveRequest(int requestId);

        Task<ContactResp> ChangeRequestStatus(int requestId);
    }
}
