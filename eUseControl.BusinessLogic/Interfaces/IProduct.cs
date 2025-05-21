using System.Collections.Generic;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProduct
    {
        ProductResp CreateProduct(ProductData productData, int userId);

        List<ProductMinimal> GetProductsByUserId(int userId);

        ProductResp UpdateProduct(ProductData productData);

        ProductData GetProductById(int productId);

        List<ProductSummary> GetAvailableProducts();

        ProductResp UpdateProductStatus(int productId);

        Dictionary<Category, int> GetCategoryProductCounts();

        List<ProductSummary> GetAvailableProductsByCategoryId(int? categoryId);

        ProductResp UpdateProductRating(int productId);

        List<ProductSummary> SortProducts(string sortOption, List<ProductSummary> products);

        List<ProductSummary> GetProductsByMaxPrice(int maxPrice, List<ProductSummary> products);

        List<ProductSummary> GetProductsBySearchQuery(string searchQuery, List<ProductSummary> products);

        List<ProductSummary> GetProductsByCountry(string country, List<ProductSummary> products);
    }
}