using System.Linq;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class ProductTests
    {
        private readonly IProduct _product;
        private readonly ICart _cart;

        public ProductTests()
        {
            var bl = new BusinessLogicManager();
            _product = bl.GetProductBL();
            _cart = bl.GetCartBL();
        }

        [TestMethod]
        public void MissingFields()
        {
            int userId = 1;
            var productData = new ProductData
            {
                ProductName = "",
                ProductAddress = "Address",
                ProductQuantity = 10,
                ProductQuality = "High",
                ProductPrice = 100,
                ProductRegion = "Region",
                ProductImageUrl = "url.jpg",
                ProductDescription = "Description",
                ProductCategory = "Fruits"
            };

            var result = _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("All fields are required!", result.StatusMsg);
        }

        [TestMethod]
        public void NegativeQuantity()
        {
            int userId = 1;
            var productData = new ProductData
            {
                ProductName = "Apple",
                ProductAddress = "Address",
                ProductQuantity = -5,
                ProductQuality = "High",
                ProductPrice = 100,
                ProductRegion = "Region",
                ProductImageUrl = "url.jpg",
                ProductDescription = "Description",
                ProductCategory = "Fruits"
            };

            var result = _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Quantity must be a positive number!", result.StatusMsg);
        }

        [TestMethod]
        public void ZeroPrice()
        {
            int userId = 1;
            var productData = new ProductData
            {
                ProductName = "Apple",
                ProductAddress = "Address",
                ProductQuantity = 10,
                ProductQuality = "High",
                ProductPrice = 0,
                ProductRegion = "Region",
                ProductImageUrl = "url.jpg",
                ProductDescription = "Description",
                ProductCategory = "Fruits"
            };

            var result = _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Price must be greater than zero!", result.StatusMsg);
        }

        [TestMethod]
        public void InvalidCategory()
        {
            int userId = 1;
            var productData = new ProductData
            {
                ProductName = "Apple",
                ProductAddress = "Address",
                ProductQuantity = 10,
                ProductQuality = "High",
                ProductPrice = 100,
                ProductRegion = "Region",
                ProductImageUrl = "url.jpg",
                ProductDescription = "Description",
                ProductCategory = "InvalidCategoryName"  
            };

            var result = _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Invalid category!", result.StatusMsg);
        }

        [TestMethod]
        public void ValidProduct()
        {
            int userId = 1;
            var productData = new ProductData
            {
                ProductName = "Pants",
                ProductAddress = "Address",
                ProductQuantity = 10,
                ProductQuality = "High",
                ProductPrice = 100,
                ProductRegion = "Region",
                ProductImageUrl = "url.jpg",
                ProductDescription = "Description",
                ProductCategory = "Clothing"
            };

            var result = _product.CreateProduct(productData, userId);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("The product has been successfully created!", result.StatusMsg);
        }

        [TestMethod]
        public void UserNotFound()
        {
            int userId = -1;

            var result = _product.GetProductsByUserId(userId);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void UserNoProducts()
        {
            int userId = 5;

            var result = _product.GetProductsByUserId(userId); 

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void UserHasProducts()
        {
            int userId = 1;

            var result = _product.GetProductsByUserId(userId);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void ProductNullFields()
        {
            var data = new ProductData
            {
                ProductName = "",
                ProductAddress = "Addr",
                ProductQuality = "High",
                ProductRegion = "North",
                ProductDescription = "Desc",
                ProductCategory = "Cat",
                ProductQuantity = 10,
                ProductPrice = 100
            };

            var result = _product.UpdateProduct(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("All fields are required!", result.StatusMsg);
        }

        [TestMethod]
        public void ProductNegativeQuantity()
        {
            var data = new ProductData
            {
                ProductName = "Prod",
                ProductAddress = "Addr",
                ProductQuality = "High",
                ProductRegion = "North",
                ProductDescription = "Desc",
                ProductCategory = "Cat",
                ProductQuantity = -5,
                ProductPrice = 100
            };

            var result = _product.UpdateProduct(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Quantity must be a positive number!", result.StatusMsg);
        }

        [TestMethod]
        public void ProductZeroPrice()
        {
            var data = new ProductData
            {
                ProductName = "Prod",
                ProductAddress = "Addr",
                ProductQuality = "High",
                ProductRegion = "North",
                ProductDescription = "Desc",
                ProductCategory = "Cat",
                ProductQuantity = 5,
                ProductPrice = 0
            };

            var result = _product.UpdateProduct(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Price must be greater than zero!", result.StatusMsg);
        }

        [TestMethod]
        public void ProductBadCategory()
        {
            var data = new ProductData
            {
                ProductName = "Prod",
                ProductAddress = "Addr",
                ProductQuality = "High",
                ProductRegion = "North",
                ProductDescription = "Desc",
                ProductCategory = "NoSuchCat",
                ProductQuantity = 5,
                ProductPrice = 100
            };

            var result = _product.UpdateProduct(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Invalid category!", result.StatusMsg);
        }

        [TestMethod]
        public void ProductNotFound()
        {
            var data = new ProductData
            {
                Id = -1,
                ProductName = "Prod",
                ProductAddress = "Addr",
                ProductQuality = "High",
                ProductRegion = "North",
                ProductDescription = "Desc",
                ProductCategory = "Clothing",
                ProductQuantity = 5,
                ProductPrice = 100
            };

            var result = _product.UpdateProduct(data);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public void ProductUpdateOk()
        {
            var data = new ProductData
            {
                Id = 4, 
                ProductName = "ProdNew",
                ProductAddress = "AddrNew",
                ProductQuality = "Low",
                ProductRegion = "South",
                ProductDescription = "Updated",
                ProductCategory = "Clothing",
                ProductQuantity = 20,
                ProductPrice = 300m,
                ProductImageUrl = "img.jpg"
            };

            var result = _product.UpdateProduct(data);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Product updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void GetByIdFound()
        {
            int productId = 1;

            var result = _product.GetProductById(productId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void GetByIdNotFound()
        {
            int productId = -1;

            var result = _product.GetProductById(productId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void AvailableProductsNotEmpty()
        {
            var result = _product.GetAvailableProducts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void StatusUpdateOk()
        {
            int productId = 4;

            var result = _product.UpdateProductStatus(productId);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Product status updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void StatusUpdateNotFound()
        {
            int productId = -1;

            var result = _product.UpdateProductStatus(productId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public void CategoryCountsNotEmpty()
        {
            var result = _product.GetCategoryProductCounts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void AvailProductsValidCategoryId()
        {
            int validCategoryId = 2;

            var result = _product.GetAvailableProductsByCategoryId(validCategoryId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void AvailProductsInvalidCategoryId()
        {
            int invalidCategoryId = -1;

            var result = _product.GetAvailableProductsByCategoryId(invalidCategoryId);

            Assert.AreEqual(0, result.Count); 
        }

        [TestMethod]
        public void UpdateRatingValidId()
        {
            int testProductId = 5;

            var result = _product.UpdateProductRating(testProductId);

            Assert.IsTrue(result.Status);
            Assert.AreEqual("Product rating updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void UpdateRatingInvalidId()
        {
            int invalidProductId = -1;

            var result = _product.UpdateProductRating(invalidProductId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public void SortProductsLowToHigh()
        {
            string sortOption = "lowToHigh";
            var products = _product.GetAvailableProducts();

            var sorted = _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted);
            Assert.IsTrue(sorted.Count > 0);
            Assert.IsTrue(sorted.First().ProductPrice <= sorted.Last().ProductPrice);
        }

        [TestMethod]
        public void SortProductsHighToLow()
        {
            string sortOption = "highToLow";
            var products = _product.GetAvailableProducts();

            var sorted = _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted);
            Assert.IsTrue(sorted.Count > 0);
            Assert.IsTrue(sorted.First().ProductPrice >= sorted.Last().ProductPrice);
        }

        [TestMethod]
        public void SortProductsNewest()
        {
            string sortOption = "newest";
            var products = _product.GetAvailableProducts();

            var sorted = _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted);
            Assert.IsTrue(sorted.Count > 0);
            Assert.IsTrue(sorted.First().ProductPostDate >= sorted.Last().ProductPostDate);
        }

        [TestMethod]
        public void SortProductsOldest()
        {
            string sortOption = "oldest";
            var products = _product.GetAvailableProducts();

            var sorted = _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted);
            Assert.IsTrue(sorted.Count > 0);
            Assert.IsTrue(sorted.First().ProductPostDate <= sorted.Last().ProductPostDate);
        }

        [TestMethod]
        public void FilterProductsByMaxPrice()
        {
            int maxPrice = 100;
            var products = _product.GetAvailableProducts();

            var filtered = _product.GetProductsByMaxPrice(maxPrice, products);

            Assert.IsNotNull(filtered);
            Assert.IsTrue(filtered.Count > 0);
            Assert.IsTrue(filtered.All(p => p.ProductPrice <= maxPrice));
        }

        [TestMethod]
        public void FilterProductsBySearchQuery()
        {
            string searchQuery = "Pants";
            var products = _product.GetAvailableProducts(); 

            var result = _product.GetProductsBySearchQuery(searchQuery, products);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void FilterProductsByCountry()
        {
            string country = "Region";
            var products = _product.GetAvailableProducts();

            var result = _product.GetProductsByCountry(country, products);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void UpdateProductQuantitySuccess()
        {
            int userId = 2;
            var cartItems = _cart.GetCartItemsByUserId(userId); 

            var result = _product.UpdateProductQuantitiesAfterOrder(cartItems); 

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status);
            Assert.AreEqual("Product quantities updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public void RecommendedProductsReturnsList()
        {
            var result = _product.GetRecommendedProducts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void RemoveProductInvalidId()
        {
            int invalidProductId = -1;

            var result = _product.RemoveProduct(invalidProductId);

            Assert.IsFalse(result.Status);
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }
    }
}
