using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ProductBL : UserApi, IProduct
    {
        public async Task<ProductResp> CreateProduct(ProductData productData, int userId)
        {
            return await CreateProductAction(productData, userId);
        }

        public async Task<List<ProductMinimal>> GetProductsByUserId(int userId)
        {
            return await GetProductsByUserIdAction(userId);
        }

        public async Task<ProductResp> UpdateProduct(ProductData productData)
        {
            return await UpdateProductAction(productData);
        }

        public async Task<ProductData> GetProductById(int productId)
        {
            return await GetProductByIdAction(productId);
        }

        public async Task<List<ProductSummary>> GetAvailableProducts()
        {
            return await GetAvailableProductsAction();
        }

        public async Task<ProductResp> UpdateProductStatus(int productId)
        {
            return await UpdateProductStatusAction(productId);
        }

        public Dictionary<CategoryData, int> GetCategoryProductCounts()
        {
            return GetCategoryProductCountsAction();
        }

        public async Task<List<ProductSummary>> GetAvailableProductsByCategoryId(int? categoryId)
        {
            return await GetAvailableProductsByCategoryIdAction(categoryId);
        }

        public async Task<ProductResp> UpdateProductRating(int productId)
        {
            return await UpdateProductRatingAction(productId);
        }

        public async Task<List<ProductSummary>> SortProducts(string sortOption, List<ProductSummary> products)
        {
            return await SortProductsAction(sortOption, products);
        }

        public async Task<List<ProductSummary>> GetProductsByMaxPrice(int maxPrice, List<ProductSummary> products)
        {
            return await GetProductsByMaxPriceAction(maxPrice, products);
        }

        public async Task<List<ProductSummary>> GetProductsBySearchQuery(string searchQuery, List<ProductSummary> products)
        {
            return await GetProductsBySearchQueryAction(searchQuery, products);
        }

        public async Task<List<ProductSummary>> GetProductsByCountry(string country, List<ProductSummary> products)
        {
            return await GetProductsByCountryAction(country, products);
        }

        public async Task<ProductResp> UpdateProductQuantitiesAfterOrder(List<CartData> cartItems)
        {
            return await UpdateProductQuantitiesAfterOrderAction(cartItems);
        }

        public async Task<List<ProductSummary>> GetRecommendedProducts()
        {
            return await GetRecommendedProductsAction();
        }

        public async Task<ProductResp> RemoveProduct(int productId)
        {
            return await RemoveProductAction(productId);
        }

        public async Task<Dictionary<string, List<ProductSummary>>> GetProductsFromTopCategories()
        {
            return await GetProductsFromTopCategoriesAction();
        }

        public async Task<List<string>> ExtractCategories()
        {
            return await ExtractCategoriesAction();
        }
    }
}
