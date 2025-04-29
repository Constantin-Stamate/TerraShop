using System.Collections.Generic;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IProduct
    {
        ProductResp CreateProduct(ProductData productData, string cookie);

        List<ProductMinimal> GetProductsByUser(string cookie);

        ProductResp UpdateProduct(ProductData productData, string cookie, int productId);

        ProductData GetProductById(int productId, string cookie);
    }
}