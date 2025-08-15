using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IReview
    {
        Task<ReviewResp> CreateReview(ReviewData data, int userId);

        Task<List<ReviewData>> GetReviewsByProductId(int productId);

        Task<ReviewResp> DeleteReview(int reviewId);

        Task<ReviewData> GetReviewById(int? reviewId);

        Task<ReviewResp> UpdateReview(ReviewData data);

        Task<Dictionary<ReviewData, ProfileData>> RetrieveAllReviews();
    }
}
