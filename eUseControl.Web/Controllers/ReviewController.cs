using System.Web.Mvc;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Review;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReview _review;
        private readonly ISession _session;
        private readonly IProduct _product;

        public ReviewController()
        {
            var bl = new BusinessLogicManager();
            _review = bl.GetReviewBL();
            _session = bl.GetSessionBL();  
            _product = bl.GetProductBL();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostReview(ProductDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cookie = Request.Cookies["X-KEY"].Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var reviewData = new ReviewData
                {
                    Id = model.ReviewCompact.Id,
                    ProductId = model.ReviewCompact.ProductId,
                    Review = model.ReviewCompact.Review,
                    Rating = model.ReviewCompact.Rating
                };

                ReviewResp result;

                if (reviewData.Id > 0)
                {
                    result = _review.UpdateReview(reviewData);
                    _product.UpdateProductRating(model.ReviewCompact.ProductId);
                }
                else
                {
                    result = _review.CreateReview(reviewData, user.Id);
                    _product.UpdateProductRating(model.ReviewCompact.ProductId);
                }

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("ProductDetails", "Product", new { productId = reviewData.ProductId, success = true });
                }
                else
                {
                    ModelState.AddModelError("", result.StatusMsg);
                    TempData["ErrorMessage"] = result.StatusMsg;
                    return RedirectToAction("ProductDetails", "Product", new { productId = reviewData.ProductId, error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The model you submitted is invalid!";
                return RedirectToAction("Shop", "Shop", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReview(int reviewId, int productId)
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var user = _session.GetUserByCookie(cookie);
            if (user == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var result = _review.DeleteReview(reviewId);
            _product.UpdateProductRating(productId);

            if (result.Status)
            {
                return RedirectToAction("ProductDetails", "Product", new { productId, success = true });
            }
            else
            {
                return RedirectToAction("ProductDetails", "Product", new { productId, error = true });
            }
        }
    }
}