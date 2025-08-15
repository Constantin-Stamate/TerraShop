using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Cart;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProduct
    {
        Task<ProductResp> CreateProduct(ProductData productData, int userId);

        Task<List<ProductMinimal>> GetProductsByUserId(int userId);

        Task<ProductResp> UpdateProduct(ProductData productData);

        Task<ProductData> GetProductById(int productId);

        Task<List<ProductSummary>> GetAvailableProducts();

        Task<ProductResp> UpdateProductStatus(int productId);

        Dictionary<CategoryData, int> GetCategoryProductCounts();

        Task<List<ProductSummary>> GetAvailableProductsByCategoryId(int? categoryId);

        Task<ProductResp> UpdateProductRating(int productId);

        Task<List<ProductSummary>> SortProducts(string sortOption, List<ProductSummary> products);

        Task<List<ProductSummary>> GetProductsByMaxPrice(int maxPrice, List<ProductSummary> products);

        Task<List<ProductSummary>> GetProductsBySearchQuery(string searchQuery, List<ProductSummary> products);

        Task<List<ProductSummary>> GetProductsByCountry(string country, List<ProductSummary> products);

        Task<ProductResp> UpdateProductQuantitiesAfterOrder(List<CartData> cartItems);

        Task<List<ProductSummary>> GetRecommendedProducts();

        Task<ProductResp> RemoveProduct(int productId);

        Task<Dictionary<string, List<ProductSummary>>> GetProductsFromTopCategories();

        Task<List<string>> ExtractCategories();
    }
}