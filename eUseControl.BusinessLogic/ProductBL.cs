using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic
{
    public class ProductBL : UserApi, IProduct
    {
        public ProductResp CreateProduct(ProductData productData, string cookie)
        {
            return CreateProductAction(productData, cookie);
        }

        public List<ProductMinimal> GetProductsByUser(string cookie)
        {
            return GetProducts(cookie);
        }

        public ProductResp UpdateProduct(ProductData productData, string cookie, int productId)
        {
            return UpdateProductAction(productData, cookie, productId);
        } 

        public ProductData GetProductById(int productId, string cookie)
        {
            return GetProductByIdAction(productId, cookie);
        }
    }
}
