using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class ProductBL : UserApi, IProduct
    {
        public ProductResp CreateProduct(ProductData productData, int userId)
        {
            return CreateProductAction(productData, userId);
        }

        public List<ProductMinimal> GetProductsByUserId(int userId)
        {
            return GetProductsByUserIdAction(userId);
        }

        public ProductResp UpdateProduct(ProductData productData)
        {
            return UpdateProductAction(productData);
        } 

        public ProductData GetProductById(int productId)
        {
            return GetProductByIdAction(productId);
        }

        public List<ProductSummary> GetAvailableProducts()
        {
            return GetAvailableProductsAction();
        }

        public void UpdateProductStatus(int productId)
        {
            UpdateProductStatusAction(productId);
        } 

        public Dictionary<Category, int> GetCategoryProductCounts()
        {
            return GetCategoryProductCountsAction();
        }

        public List<ProductSummary> GetAvailableProductsByCategoryId(int? categoryId)
        {
            return GetAvailableProductsByCategoryIdAction(categoryId);
        }

        public void UpdateProductRating(int productId)
        {
            UpdateProductRatingAction(productId);
        }
    }
}
