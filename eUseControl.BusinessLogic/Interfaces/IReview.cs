using System.Collections.Generic;
using eUseControl.Domain.Entities.Review;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IReview
    {
        ReviewResp CreateReview(ReviewData data, int userId);

        List<ReviewData> GetReviewsByProductId(int productId);

        ReviewResp DeleteReview(int reviewId);

        ReviewData GetReviewById(int? reviewId);

        ReviewResp UpdateReview(ReviewData data);
    }
}
