using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IProduct _product;

        public ShopController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
        }

        [HttpGet]
        public ActionResult Shop(int? categoryId, int page = 1)
        {
            var cookie = Request.Cookies["X-KEY"].Value;
            if (string.IsNullOrEmpty(cookie))
            {
                return RedirectToAction("Login", "Login", new { error = true });
            }

            List<ProductSummary> productsList;

            if (categoryId == 0 || !categoryId.HasValue)
            {
                productsList = _product.GetAvailableProducts();

            }
            else
            {
                productsList = _product.GetAvailableProductsByCategoryId(categoryId);
            }

            var categoryProductCounts = _product.GetCategoryProductCounts();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductSummary, ProductMini>();
                cfg.CreateMap<Category, ProductCategory>();
            });

            var mapper = config.CreateMapper();

            var products = mapper.Map<List<ProductMini>>(productsList);
            var productCountsByCategory = mapper.Map<Dictionary<ProductCategory, int>>(categoryProductCounts);

            int pageSize = 12;
            var totalProducts = productsList.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var productsForCurrentPage = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new ProductCatalogViewModel
            {
                Products = productsForCurrentPage,
                Categories = productCountsByCategory,
                CurrentPage = page,
                TotalPages = totalPages,
                CategoryId = categoryId
            };

            return View(model);
        }
    }
}