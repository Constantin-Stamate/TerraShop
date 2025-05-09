using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Profile;
using eUseControl.Domain.Entities.Review;
using eUseControl.Web.Models.Product;
using eUseControl.Web.Models.User;
using eUseControl.Web.Models.Profile;
using eUseControl.Web.Models.Review;

namespace eUseControl.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProduct _product;
        private readonly ISession _session;
        private readonly IReview _review;
        private readonly IProfile _profile;

        public ProductController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _session = bl.GetSessionBL();
            _review = bl.GetReviewBL();
            _profile = bl.GetProfileBL();
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product product, HttpPostedFileBase productImageUrl)
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
                            productImageUrl.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("AddProduct", "Product", new { error = true });
                        }
                    }

                    product.ProductImageUrl = "~/Uploads/products/" + fileName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Product image is required!";
                    return RedirectToAction("AddProduct", "Product", new { error = true });
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductData>();
                });

                var mapper = config.CreateMapper();
                var productData = mapper.Map<ProductData>(product);

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

                var result = _product.CreateProduct(productData, user.Id);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("AddProduct", "Product", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", result.StatusMsg);
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
        public ActionResult UpdateProduct(int Id)
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

            var productData = _product.GetProductById(Id);
            if (productData == null)
            {
                return RedirectToAction("ArticlesProfile", "Profile", new { error = true });
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductData, Product>();
            });

            var mapper = config.CreateMapper();
            var product = mapper.Map<Product>(productData);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProduct(Product product, HttpPostedFileBase productImageUrl)
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
                            productImageUrl.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("UpdateProduct", "Product", new { error = true });
                        }
                    }

                    product.ProductImageUrl = "~/Uploads/products/" + fileName;
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductData>();
                });

                var mapper = config.CreateMapper();
                var productData = mapper.Map<ProductData>(product);

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

                var result = _product.UpdateProduct(productData);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = result.StatusMsg;
                    return RedirectToAction("UpdateProduct", "Product", new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", result.StatusMsg);
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
        public ActionResult ProductDetails(int productId, int? reviewId)
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var userMinimal = _session.GetUserByCookie(cookie);
            if (userMinimal == null)
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var productData = _product.GetProductById(productId);
            if (productData == null)
            {
                return RedirectToAction("Shop", "Shop", new { error = true });
            }

            var allReviews = _review.GetReviewsByProductId(productId);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewData, ReviewCompact>();
                cfg.CreateMap<ProfileData, ProfileMini>();
                cfg.CreateMap<ProductData, Product>();
                cfg.CreateMap<UserMinimal, UserCompact>();
            });

            var mapper = config.CreateMapper();

            var reviewProfileDict = new Dictionary<ReviewCompact, ProfileMini>();

            foreach (var reviewData in allReviews)
            {
                var review = mapper.Map<ReviewCompact>(reviewData);
                var profileData = _profile.GetProfileByUserId(reviewData.UserId);
                if (profileData != null)
                {
                    var profile = mapper.Map<ProfileMini>(profileData);
                    reviewProfileDict.Add(review, profile);
                }
            }

            var product = mapper.Map<Product>(productData);
            var user = mapper.Map<UserCompact>(userMinimal);

            ReviewCompact reviewToEdit;
            if (reviewId.HasValue && reviewId != 0)
            {
                var reviewData = _review.GetReviewById(reviewId);
                reviewToEdit = mapper.Map<ReviewCompact>(reviewData);
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

            var model = new ProductDetailsViewModel
            {
                Product = product,
                UserCompact = user,
                ReviewCompact = reviewToEdit,
                Reviews = reviewProfileDict
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProductStatus(int productId)
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            _product.UpdateProductStatus(productId);
            return RedirectToAction("Articlesprofile", "Profile", new { success = true });
        }
    }
}