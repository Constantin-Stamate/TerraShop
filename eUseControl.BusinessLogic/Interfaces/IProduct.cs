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

        void UpdateProductStatus(int productId);

        Dictionary<Category, int> GetCategoryProductCounts();

        List<ProductSummary> GetAvailableProductsByCategoryId(int? categoryId);

        void UpdateProductRating(int productId);
    }
}