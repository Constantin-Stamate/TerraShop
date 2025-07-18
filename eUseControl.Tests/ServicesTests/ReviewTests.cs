using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Review;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ReviewTests
    {
        private readonly IReview _review;

        public ReviewTests()
        {
            var bl = new BusinessLogicManager();
            _review = bl.GetReviewBL();
        }

        [TestMethod]
        public void ReviewEmptyText()
        {
            int userId = 1;
            var data = new ReviewData
            {
                ProductId = 1,
                Review = "",
                Rating = 5
            };

            var result = _review.CreateReview(data, userId);

            Assert.IsFalse(result.Status, "Expected failure when review text is empty!");
            Assert.AreEqual("Please enter a review before submitting!", result.StatusMsg);
        }

        [TestMethod]
        public void ReviewInvalidRating()
        {
            int userId = 1;
            var data = new ReviewData
            {
                ProductId = 1,
                Review = "Great product!",
                Rating = 0
            };

            var result = _review.CreateReview(data, userId);

            Assert.IsFalse(result.Status, "Expected failure when rating is invalid!");
            Assert.AreEqual("Please select a rating for the product before submitting your review!", result.StatusMsg);
        }

        [TestMethod]
        public void ReviewSuccess()
        {
            int userId = 1;
            var data = new ReviewData
            {
                ProductId = 1,
                Review = "Very good quality!",
                Rating = 5
            };

            var result = _review.CreateReview(data, userId);

            Assert.IsTrue(result.Status, "Expected success when valid review and rating are submitted!");
            Assert.AreEqual("Your review has been successfully created!", result.StatusMsg);
        }

        [TestMethod]
        public void NoReviewsFound()
        {
            int productId = -1;

            var result = _review.GetReviewsByProductId(productId);

            Assert.IsNotNull(result, "Expected non-null list even if no reviews found!");
            Assert.AreEqual(0, result.Count, "Expected zero reviews for invalid product ID!");
        }

        [TestMethod]
        public void ReviewsReturned()
        {
            int productId = 1;

            var result = _review.GetReviewsByProductId(productId);

            Assert.IsNotNull(result, "Expected non-null list of reviews!");
            Assert.IsTrue(result.Count > 0, "Expected at least one review for this product!");
        }

        [TestMethod]
        public void DeleteReviewInvalidId()
        {
            int reviewId = -1;

            var result = _review.DeleteReview(reviewId);

            Assert.IsFalse(result.Status, "Expected failure when deleting with invalid review ID!");
            Assert.AreEqual("Hmm... we couldn't find the review you were trying to delete!", result.StatusMsg);
        }

        [TestMethod]
        public void DeleteReviewSuccess()
        {
            int reviewId = 1;

            var result = _review.DeleteReview(reviewId);

            Assert.IsTrue(result.Status, "Expected success when deleting existing review!");
            Assert.AreEqual("Your review has been successfully deleted!", result.StatusMsg);
        }

        [TestMethod]
        public void GetReviewInvalidId()
        {
            int? reviewId = -1;

            var result = _review.GetReviewById(reviewId);

            Assert.IsNull(result, "Expected null result for invalid review ID!");
        }

        [TestMethod]
        public void GetReviewSuccess()
        {
            int? reviewId = 2;

            var result = _review.GetReviewById(reviewId);

            Assert.IsNotNull(result, "Expected to find review with valid ID!");
            Assert.AreEqual(reviewId, result.Id, "Expected the returned review ID to match the requested ID!");
            Assert.IsFalse(string.IsNullOrEmpty(result.Review), "Expected review text to be present!");
        }

        [TestMethod]
        public void UpdateReviewEmptyReviewText()
        {
            var review = new ReviewData
            {
                Id = 1,
                Review = "",
                Rating = 4
            };

            var result = _review.UpdateReview(review);

            Assert.IsFalse(result.Status, "Expected failure when updating with empty review text!");
            Assert.AreEqual("Please enter a review before submitting!", result.StatusMsg);
        }

        [TestMethod]
        public void UpdateReviewInvalidRating()
        {
            var review = new ReviewData
            {
                Id = 1,
                Review = "Good product!",
                Rating = 0
            };

            var result = _review.UpdateReview(review);

            Assert.IsFalse(result.Status, "Expected failure when updating with invalid rating!");
            Assert.AreEqual("Please select a rating for the product before submitting your review!", result.StatusMsg);
        }

        [TestMethod]
        public void UpdateReviewNotFound()
        {
            var review = new ReviewData
            {
                Id = -1,
                Review = "Updated review",
                Rating = 5
            };

            var result = _review.UpdateReview(review);

            Assert.IsFalse(result.Status, "Expected failure when updating a non-existent review!");
            Assert.AreEqual("Hmm... we couldn't find the review you were trying to update!", result.StatusMsg);
        }

        [TestMethod]
        public void UpdateReviewSuccess()
        {
            var review = new ReviewData
            {
                Id = 2,
                Review = "Updated content for review",
                Rating = 5
            };

            var result = _review.UpdateReview(review);

            Assert.IsTrue(result.Status, "Expected success when updating a valid review!");
            Assert.AreEqual("Your review has been successfully updated!", result.StatusMsg);
        }

        [TestMethod]
        public void RetrieveAllReviewsSuccess()
        {
            var result = _review.RetrieveAllReviews();

            Assert.IsNotNull(result, "Expected non-null list of all reviews!");
            Assert.IsTrue(result.Count > 0, "Expected at least one review with profile!");
        }
    }
}
