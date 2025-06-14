﻿using System;
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
        private readonly IWishlist _wishlist;
        private readonly ISession _session;

        public ShopController()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _wishlist = bl.GetWishlistBL();
            _session = bl.GetSessionBL();
        }

        [HttpGet]
        public ActionResult Shop(string country, string searchQuery, string maxPrice, string sortOption, int? categoryId, int page = 1)
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

            List<ProductSummary> productsList;
            int value = 0;

            if (!string.IsNullOrEmpty(maxPrice) && maxPrice != "0")
            {
                value = int.Parse(maxPrice);
            }

            if (categoryId == 0 || !categoryId.HasValue)
            {
                productsList = _product.GetAvailableProducts();
            }
            else
            {
                productsList = _product.GetAvailableProductsByCategoryId(categoryId);
            }

            if (value != 0)
            {
                productsList = _product.GetProductsByMaxPrice(value, productsList);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                productsList = _product.GetProductsBySearchQuery(searchQuery, productsList);
            }

            if (!string.IsNullOrEmpty(country))
            {
                productsList = _product.GetProductsByCountry(country, productsList);
            }

            productsList = _product.SortProducts(sortOption, productsList);

            var categoryProductCounts = _product.GetCategoryProductCounts();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductSummary, ProductMini>();
                cfg.CreateMap<CategoryDbTable, ProductCategory>();
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

            var productIds = _wishlist.GetWishlistProductIds(user.Id);

            var model = new ProductCatalogViewModel
            {
                Products = productsForCurrentPage,
                Categories = productCountsByCategory,
                CurrentPage = page,
                TotalPages = totalPages,
                CategoryId = categoryId,
                SortOption = sortOption,
                MaxPrice = value,
                SearchQuery = searchQuery,
                Country = country,
                WishlistProductIds = productIds
            };

            return View(model);
        }
    }
}