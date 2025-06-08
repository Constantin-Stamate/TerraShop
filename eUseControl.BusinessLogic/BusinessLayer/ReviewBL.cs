using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ReviewBL : UserApi, IReview
    {
        public ReviewResp CreateReview(ReviewData data, int userId)
        {
            return CreateReviewAction(data, userId);
        }

        public List<ReviewData> GetReviewsByProductId(int productId)
        {
            return GetReviewsByProductIdAction(productId);
        }

        public ReviewResp DeleteReview(int reviewId)
        {
            return DeleteReviewAction(reviewId);
        }

        public ReviewData GetReviewById(int? reviewId)
        {
            return GetReviewByIdAction(reviewId);
        }

        public ReviewResp UpdateReview(ReviewData data)
        {
            return UpdateReviewAction(data);
        }

        public Dictionary<ReviewData, ProfileData> RetrieveAllReviews()
        {
            return RetrieveAllReviewsAction();
        }
    }
}
