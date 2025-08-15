using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ReviewBL : UserApi, IReview
    {
        public async Task<ReviewResp> CreateReview(ReviewData data, int userId)
        {
            return await CreateReviewAction(data, userId);
        }

        public async Task<List<ReviewData>> GetReviewsByProductId(int productId)
        {
            return await GetReviewsByProductIdAction(productId);
        }

        public async Task<ReviewResp> DeleteReview(int reviewId)
        {
            return await DeleteReviewAction(reviewId);
        }

        public async Task<ReviewData> GetReviewById(int? reviewId)
        {
            return await GetReviewByIdAction(reviewId);
        }

        public async Task<ReviewResp> UpdateReview(ReviewData data)
        {
            return await UpdateReviewAction(data);
        }

        public async Task<Dictionary<ReviewData, ProfileData>> RetrieveAllReviews()
        {
            return await RetrieveAllReviewsAction();
        }
    }
}
