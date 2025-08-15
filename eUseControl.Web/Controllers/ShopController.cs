using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IProduct _product;
        private readonly IWishlist _wishlist;
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public ShopController(IMapper mapper)
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _wishlist = bl.GetWishlistBL();
            _session = bl.GetSessionBL();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Shop(string country, string searchQuery, string maxPrice, string sortOption, int? categoryId, int page = 1)
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

            int.TryParse(maxPrice, out var value);

            var productsList = (categoryId == 0 || !categoryId.HasValue) ? await _product.GetAvailableProducts() : await _product.GetAvailableProductsByCategoryId(categoryId);

            if (value > 0)
            {
                productsList = await _product.GetProductsByMaxPrice(value, productsList);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                productsList = await _product.GetProductsBySearchQuery(searchQuery, productsList);
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                productsList = await _product.GetProductsByCountry(country, productsList);
            }

            productsList = await _product.SortProducts(sortOption, productsList);

            var categoryProductCounts = _product.GetCategoryProductCounts();

            var allRecommendedProducts = await _product.GetRecommendedProducts();

            var products = _mapper.Map<List<ProductMini>>(productsList);
            var productCountsByCategory = _mapper.Map<Dictionary<ProductCategory, int>>(categoryProductCounts);
            var recommendedProducts = _mapper.Map<List<ProductMini>>(allRecommendedProducts);

            const int pageSize = 12;
            var totalPages = (int)Math.Ceiling(productsList.Count / (double)pageSize);
            var productsForCurrentPage = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productIds = await _wishlist.GetWishlistProductIds(user.Id);

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
                WishlistProductIds = productIds,
                RecommendedProducts = recommendedProducts
            };

            return View(model);
        }
    }
}