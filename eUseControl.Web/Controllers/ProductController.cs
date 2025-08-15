using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models.Product;
using eUseControl.Web.Models.Profile;
using eUseControl.Web.Models.Review;
using eUseControl.Web.Models.User;

namespace eUseControl.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProduct _product;
        private readonly ISession _session;
        private readonly IReview _review;
        private readonly IProfile _profile;
        private readonly IWishlist _wishlist;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _session = bl.GetSessionBL();
            _review = bl.GetReviewBL();
            _profile = bl.GetProfileBL();
            _wishlist = bl.GetWishlistBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> AddProduct()
        {
            var categories = await _product.ExtractCategories();

            var model = new AddProductViewModel
            {
                Categories = categories,
                Product = new Product()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProduct(AddProductViewModel model, HttpPostedFileBase productImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (productImageUrl != null && productImageUrl.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(productImageUrl.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessage"] = "Invalid image format!";
                        return RedirectToAction("AddProduct", "Product", new { error = true });
                    }

                    string fileName = Path.GetFileName(productImageUrl.FileName);
                    string uploadsPath = Server.MapPath("~/Uploads/products/");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    string filePath = Path.Combine(uploadsPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            using (var fileStream = System.IO.File.Create(filePath))
                            {
                                await productImageUrl.InputStream.CopyToAsync(fileStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("AddProduct", "Product", new { error = true });
                        }
                    }

                    model.Product.ProductImageUrl = "~/Uploads/products/" + fileName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Product image is required!";
                    return RedirectToAction("AddProduct", "Product", new { error = true });
                }

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

                var productData = _mapper.Map<ProductData>(model.Product);

                var result = await _product.CreateProduct(productData, user.Id);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("AddProduct", "Product", new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = result.StatusMsg;
                    return RedirectToAction("AddProduct", "Product", new { error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The model you submitted is invalid!";
                return RedirectToAction("AddProduct", "Product", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateProduct(int Id)
        {
            var productData = await _product.GetProductById(Id);

            var product = _mapper.Map<Product>(productData);

            var categories = await _product.ExtractCategories();

            var model = new AddProductViewModel
            {
                Categories = categories,
                Product = product
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProduct(AddProductViewModel model, HttpPostedFileBase productImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (productImageUrl != null && productImageUrl.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(productImageUrl.FileName)?.ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessage"] = "Invalid image format!";
                        return RedirectToAction("UpdateProduct", "Product", new { error = true });
                    }

                    string fileName = Path.GetFileName(productImageUrl.FileName);
                    string uploadsPath = Server.MapPath("~/Uploads/products/");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    string filePath = Path.Combine(uploadsPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            using (var fileStream = System.IO.File.Create(filePath))
                            {
                                await productImageUrl.InputStream.CopyToAsync(fileStream);
                            }
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("UpdateProduct", "Product", new { error = true });
                        }
                    }

                    model.Product.ProductImageUrl = "~/Uploads/products/" + fileName;
                }

                var productData = _mapper.Map<ProductData>(model.Product);

                var result = await _product.UpdateProduct(productData);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("UpdateProduct", "Product", new { success = true });
                }
                else
                {
                    TempData["ErrorMessage"] = result.StatusMsg;
                    return RedirectToAction("UpdateProduct", "Product", new { error = true });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "The model you submitted is invalid!";
                return RedirectToAction("UpdateProduct", "Product", new { error = true });
            }
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetails(int productId, int? reviewId)
        {
            var cookie = Request.Cookies["X-KEY"]?.Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var userMinimal = _session.GetUserByCookie(cookie);
            if (userMinimal == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var activeUser = _mapper.Map<UserCompact>(userMinimal);

            var productData = await _product.GetProductById(productId);
            if (productData == null)
            {
                return RedirectToAction("Shop", "Shop", new { error = true });
            }

            var product = _mapper.Map<Product>(productData);

            var userData = await _session.GetUserById(productData.UserId);
            if (userData == null)
            {
                return RedirectToAction("Shop", "Shop", new { error = true });
            }

            var user = _mapper.Map<UserCompact>(userData);

            var allRecommendedProducts = await _product.GetRecommendedProducts();

            var recommendedProducts = _mapper.Map<List<ProductMini>>(allRecommendedProducts);

            var allReviews = await _review.GetReviewsByProductId(productId);

            var reviewProfileDict = new Dictionary<ReviewCompact, ProfileMini>();

            foreach (var reviewData in allReviews)
            {
                var review = _mapper.Map<ReviewCompact>(reviewData);

                var profileData = await _profile.GetProfileByUserId(reviewData.UserId);
                if (profileData != null)
                {
                    var profile = _mapper.Map<ProfileMini>(profileData);
                    reviewProfileDict.Add(review, profile);
                }
            }

            ReviewCompact reviewToEdit;

            if (reviewId.HasValue && reviewId != 0)
            {
                var reviewData = await _review.GetReviewById(reviewId);
                reviewToEdit = _mapper.Map<ReviewCompact>(reviewData);
            }
            else
            {
                reviewToEdit = new ReviewCompact
                {
                    ProductId = productId,
                    Username = user.Username,
                    Email = user.Email
                };
            }

            var productIds = await _wishlist.GetWishlistProductIds(user.Id);

            var model = new ProductDetailsViewModel
            {
                Product = product,
                UserCompact = user,
                ReviewCompact = reviewToEdit,
                Reviews = reviewProfileDict,
                SessionUser = activeUser,
                RecommendedProducts = recommendedProducts,
                WishlistProductIds = productIds
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProductStatus(int productId)
        {
            var result = await _product.UpdateProductStatus(productId);

            if (result.Status)
            {
                return RedirectToAction("Articlesprofile", "Profile", new { success = true });
            }
            else
            {
                return RedirectToAction("Articlesprofile", "Profile", new { error = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var result = await _product.RemoveProduct(productId);

            if (result.Status)
            {
                return RedirectToAction("Articlesprofile", "Profile", new { success = true });
            }
            else
            {
                return RedirectToAction("Articlesprofile", "Profile", new { error = true });
            }
        }
    }
}