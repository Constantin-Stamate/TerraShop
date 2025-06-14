﻿using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Cart;
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

        public ProductResp UpdateProductStatus(int productId)
        {
            return UpdateProductStatusAction(productId);
        } 

        public Dictionary<CategoryDbTable, int> GetCategoryProductCounts()
        {
            return GetCategoryProductCountsAction();
        }

        public List<ProductSummary> GetAvailableProductsByCategoryId(int? categoryId)
        {
            return GetAvailableProductsByCategoryIdAction(categoryId);
        }

        public ProductResp UpdateProductRating(int productId)
        {
            return UpdateProductRatingAction(productId);
        }

        public List<ProductSummary> SortProducts(string sortOption, List<ProductSummary> products)
        {
            return SortProductsAction(sortOption, products);
        }

        public List<ProductSummary> GetProductsByMaxPrice(int maxPrice, List<ProductSummary> products)
        {
            return GetProductsByMaxPriceAction(maxPrice, products);
        }

        public List<ProductSummary> GetProductsBySearchQuery(string searchQuery, List<ProductSummary> products)
        {
            return GetProductsBySearchQueryAction(searchQuery, products);
        }

        public List<ProductSummary> GetProductsByCountry(string country, List<ProductSummary> products)
        {
            return GetProductsByCountryAction(country, products);
        }

        public ProductResp UpdateProductQuantitiesAfterOrder(List<CartData> cartItems)
        {
            return UpdateProductQuantitiesAfterOrderAction(cartItems);
        }
    }
}
