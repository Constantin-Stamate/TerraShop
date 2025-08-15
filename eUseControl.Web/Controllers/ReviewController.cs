using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ReviewController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _review = bl.GetReviewBL();
            _session = bl.GetSessionBL();
            _product = bl.GetProductBL();
            _mapper = mapper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostReview(ProductDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cookie = Request.Cookies["X-KEY"]?.Value;
                if (string.IsNullOrEmpty(cookie))
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var user = _session.GetUserByCookie(cookie);
                if (user == null)
                {
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var reviewData = _mapper.Map<ReviewData>(model.ReviewCompact);

                var result = reviewData.Id > 0 ? await _review.UpdateReview(reviewData) : await _review.CreateReview(reviewData, user.Id);

                var status = await _product.UpdateProductRating(model.ReviewCompact.ProductId);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("ProductDetails", "Product", new { productId = reviewData.ProductId, success = true });
                }
                else
                {
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
        public async Task<ActionResult> DeleteReview(int reviewId, int productId)
        {
            var result = await _review.DeleteReview(reviewId);

            var status = await _product.UpdateProductRating(productId);

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