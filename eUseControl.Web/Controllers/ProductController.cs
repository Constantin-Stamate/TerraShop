using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models;

namespace eUseControl.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProduct _product;

        public ProductController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
        }

        // GET: AddProduct
        public ActionResult AddProduct()
        {
            return View();
        }

        // GET: UpdateProduct
        [HttpGet]
        public ActionResult UpdateProduct(int Id)
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                TempData["ErrorMessage"] = "You are not authenticated!";
                return RedirectToAction("Login", "Login", new { error = true });
            }

            var productData = _product.GetProductById(Id, cookie);

            if (productData == null)
            {
                TempData["ErrorMessage"] = "Product not found!";
                return RedirectToAction("UpdateProduct", "Product");
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
        public ActionResult UpdateProduct(Product product, HttpPostedFileBase ProductImageUrl, int Id)
        {
            if (ModelState.IsValid)
            {
                if (ProductImageUrl != null && ProductImageUrl.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ProductImageUrl.FileName);
                    string uploadsPath = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    string filePath = Path.Combine(uploadsPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            ProductImageUrl.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("UpdateProduct", "Product", new { error = true });
                        }
                    }

                    product.ProductImageUrl = "~/Uploads/" + fileName;
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
                    TempData["ErrorMessage"] = "You are not authenticated!";
                    return RedirectToAction("Login", "Login", new { error = true });
                }

                var result = _product.UpdateProduct(productData, cookie, Id);

                if (result.Status)
                {
                    return RedirectToAction("ArticlesProfile", "Profile", new { success = true });
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

        // GET: ProductDetails
        public ActionResult ProductDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product product, HttpPostedFileBase ProductImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ProductImageUrl != null && ProductImageUrl.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ProductImageUrl.FileName);
                    string uploadsPath = Server.MapPath("~/Uploads/");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    string filePath = Path.Combine(uploadsPath, fileName);

                    if (!System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            ProductImageUrl.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "Oops! Couldn't save the image: " + ex.Message;
                            return RedirectToAction("AddProduct", "Product", new { error = true });
                        }
                    }

                    product.ProductImageUrl = "~/Uploads/" + fileName;
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
                    TempData["ErrorMessage"] = "You are not authenticated!";
                    return RedirectToAction("Login", "Login", new { error = true }); 
                }

                var result = _product.CreateProduct(productData, cookie);

                if (result.Status)
                {
                    TempData["SuccessMessage"] = "The product has been successfully created!";
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
    }
}