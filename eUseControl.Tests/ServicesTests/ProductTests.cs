using System.Linq;
using System.Threading.Tasks;
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
        public async Task MissingFields()
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

            var result = await _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false for missing fields!");
            Assert.AreEqual("All fields are required!", result.StatusMsg);
        }

        [TestMethod]
        public async Task NegativeQuantity()
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

            var result = await _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false for negative quantity!");
            Assert.AreEqual("Quantity must be a positive number!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ZeroPrice()
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

            var result = await _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false for zero price!");
            Assert.AreEqual("Price must be greater than zero!", result.StatusMsg);
        }

        [TestMethod]
        public async Task InvalidCategory()
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

            var result = await _product.CreateProduct(productData, userId);

            Assert.IsFalse(result.Status, "Expected Status to be false for invalid category!");
            Assert.AreEqual("Invalid category!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ValidProduct()
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

            var result = await _product.CreateProduct(productData, userId);

            Assert.IsTrue(result.Status, "Expected Status to be true for valid product!");
            Assert.AreEqual("The product has been successfully created!", result.StatusMsg);
        }

        [TestMethod]
        public async Task UserNotFound()
        {
            int userId = -1;

            var result = await _product.GetProductsByUserId(userId);

            Assert.AreEqual(0, result.Count, "Expected no products for non-existent user ID!");
        }

        [TestMethod]
        public async Task UserNoProducts()
        {
            int userId = 5;

            var result = await _product.GetProductsByUserId(userId);

            Assert.AreEqual(0, result.Count, "Expected no products for user without listings!");
        }

        [TestMethod]
        public async Task UserHasProducts()
        {
            int userId = 1;

            var result = await _product.GetProductsByUserId(userId);

            Assert.IsTrue(result.Count > 0, "Expected user to have at least one product!");
        }

        [TestMethod]
        public async Task ProductNullFields()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsFalse(result.Status, "Expected status to be false for missing required fields!");
            Assert.AreEqual("All fields are required!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProductNegativeQuantity()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsFalse(result.Status, "Expected failure when quantity is negative!");
            Assert.AreEqual("Quantity must be a positive number!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProductZeroPrice()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsFalse(result.Status, "Expected failure when price is zero!");
            Assert.AreEqual("Price must be greater than zero!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProductBadCategory()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsFalse(result.Status, "Expected Status to be false for invalid category!");
            Assert.AreEqual("Invalid category!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProductNotFound()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsFalse(result.Status, "Expected Status to be false for non-existent product!");
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public async Task ProductUpdateOk()
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

            var result = await _product.UpdateProduct(data);

            Assert.IsTrue(result.Status, "Expected Status to be true for successful update!");
            Assert.AreEqual("Product updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task GetByIdFound()
        {
            int productId = 1;

            var result = await _product.GetProductById(productId);

            Assert.IsNotNull(result, "Expected product to be found with ID 1!");
            Assert.AreEqual(1, result.Id, "Expected ID to be 1!");
        }

        [TestMethod]
        public async Task GetByIdNotFound()
        {
            int productId = -1;

            var result = await _product.GetProductById(productId);

            Assert.IsNull(result, "Expected result to be null for invalid product ID!");
        }

        [TestMethod]
        public async Task AvailableProductsNotEmpty()
        {
            var result = await _product.GetAvailableProducts();

            Assert.IsNotNull(result, "Expected non-null list of available products!");
            Assert.IsTrue(result.Count > 0, "Expected at least one available product!");
        }

        [TestMethod]
        public async Task StatusUpdateOk()
        {
            int productId = 4;

            var result = await _product.UpdateProductStatus(productId);

            Assert.IsTrue(result.Status, "Expected Status to be true after status update!");
            Assert.AreEqual("Product status updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task StatusUpdateNotFound()
        {
            int productId = -1;

            var result = await _product.UpdateProductStatus(productId);

            Assert.IsFalse(result.Status, "Expected Status to be false when updating status for non-existent product!");
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public void CategoryCountsNotEmpty()
        {
            var result = _product.GetCategoryProductCounts();

            Assert.IsNotNull(result, "Expected category product count result to be not null!");
            Assert.IsTrue(result.Count > 0, "Expected category product count to be greater than 0!");
        }

        [TestMethod]
        public async Task AvailProductsValidCategoryId()
        {
            int validCategoryId = 2;

            var result = await _product.GetAvailableProductsByCategoryId(validCategoryId);

            Assert.IsNotNull(result, "Expected result to be not null for valid category ID!");
            Assert.IsTrue(result.Count > 0, "Expected at least one product for valid category ID!");
        }

        [TestMethod]
        public async Task AvailProductsInvalidCategoryId()
        {
            int invalidCategoryId = -1;

            var result = await _product.GetAvailableProductsByCategoryId(invalidCategoryId);

            Assert.AreEqual(0, result.Count, "Expected no products for invalid category ID!");
        }

        [TestMethod]
        public async Task UpdateRatingValidId()
        {
            int testProductId = 5;

            var result = await _product.UpdateProductRating(testProductId);

            Assert.IsTrue(result.Status, "Expected Status to be true for valid product ID!");
            Assert.AreEqual("Product rating updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task UpdateRatingInvalidId()
        {
            int invalidProductId = -1;

            var result = await _product.UpdateProductRating(invalidProductId);

            Assert.IsFalse(result.Status, "Expected Status to be false for invalid product ID!");
            Assert.AreEqual("Product not found!", result.StatusMsg);
        }

        [TestMethod]
        public async Task SortProductsLowToHigh()
        {
            string sortOption = "lowToHigh";
            var products = await _product.GetAvailableProducts();

            var sorted = await _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted, "Expected sorted list to be not null!");
            Assert.IsTrue(sorted.Count > 0, "Expected sorted list to have at least one element!");
            Assert.IsTrue(sorted.First().ProductPrice <= sorted.Last().ProductPrice, "Expected products to be sorted in ascending order by price!");
        }

        [TestMethod]
        public async Task SortProductsHighToLow()
        {
            string sortOption = "highToLow";
            var products = await _product.GetAvailableProducts();

            var sorted = await _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted, "Expected sorted list to be not null!");
            Assert.IsTrue(sorted.Count > 0, "Expected sorted list to have at least one element!");
            Assert.IsTrue(sorted.First().ProductPrice >= sorted.Last().ProductPrice, "Expected products to be sorted in descending order by price!");
        }

        [TestMethod]
        public async Task SortProductsNewest()
        {
            string sortOption = "newest";
            var products = await _product.GetAvailableProducts();

            var sorted = await _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted, "Expected sorted list to be not null!");
            Assert.IsTrue(sorted.Count > 0, "Expected at least one product in sorted list!");
            Assert.IsTrue(sorted.First().ProductPostDate >= sorted.Last().ProductPostDate, "Expected products to be sorted from newest to oldest!");
        }

        [TestMethod]
        public async Task SortProductsOldest()
        {
            string sortOption = "oldest";
            var products = await _product.GetAvailableProducts();

            var sorted = await _product.SortProducts(sortOption, products);

            Assert.IsNotNull(sorted, "Expected sorted list to be not null!");
            Assert.IsTrue(sorted.Count > 0, "Expected at least one product in sorted list!");
            Assert.IsTrue(sorted.First().ProductPostDate <= sorted.Last().ProductPostDate, "Expected products to be sorted from oldest to newest!");
        }

        [TestMethod]
        public async Task FilterProductsByMaxPrice()
        {
            int maxPrice = 100;
            var products = await _product.GetAvailableProducts();

            var filtered = await _product.GetProductsByMaxPrice(maxPrice, products);

            Assert.IsNotNull(filtered, "Expected filtered list to be not null!");
            Assert.IsTrue(filtered.Count > 0, "Expected at least one product with price <= maxPrice!");
            Assert.IsTrue(filtered.All(p => p.ProductPrice <= maxPrice), "All products should have price <= maxPrice!");
        }

        [TestMethod]
        public async Task FilterProductsBySearchQuery()
        {
            string searchQuery = "Pants";
            var products = await _product.GetAvailableProducts();

            var result = await _product.GetProductsBySearchQuery(searchQuery, products);

            Assert.IsNotNull(result, "Expected search result list to be not null!");
            Assert.IsTrue(result.Count > 0, "Expected at least one product matching the search query!");
        }

        [TestMethod]
        public async Task FilterProductsByCountry()
        {
            string country = "Region";
            var products = await _product.GetAvailableProducts();

            var result = await _product.GetProductsByCountry(country, products);

            Assert.IsNotNull(result, "Expected result to be not null for country filter!");
            Assert.IsTrue(result.Count > 0, "Expected at least one product matching the country filter!");
        }

        [TestMethod]
        public async Task UpdateProductQuantitySuccess()
        {
            int userId = 3;
            var cartItems = await _cart.GetCartItemsByUserId(userId);

            var result = await _product.UpdateProductQuantitiesAfterOrder(cartItems);

            Assert.IsNotNull(result, "Expected result object to be not null!");
            Assert.IsTrue(result.Status, "Expected Status to be true after updating quantities!");
            Assert.AreEqual("Product quantities updated successfully!", result.StatusMsg);
        }

        [TestMethod]
        public async Task RecommendedProductsReturnsList()
        {
            var result = await _product.GetRecommendedProducts();

            Assert.IsNotNull(result, "Expected recommended products list to be not null!");
            Assert.IsTrue(result.Count > 0, "Expected at least one recommended product!");
        }

        [TestMethod]
        public async Task RemoveProductInvalidId()
        {
            int invalidProductId = -1;

            var result = await _product.RemoveProduct(invalidProductId);

            Assert.IsFalse(result.Status, "Expected Status to be false for invalid product ID!");
            Assert.AreEqual("Product not found!", result.StatusMsg, "Expected message for non-existent product!");
        }

        [TestMethod]
        public async Task GetTopCategoriesSuccess()
        {
            var result = await _product.GetProductsFromTopCategories();

            Assert.IsNotNull(result, "Expected non-null dictionary!");
            Assert.IsTrue(result.Count > 0, "Expected at least one top category with products!");
        }

        [TestMethod]
        public async Task ExtractCategoriesSuccess()
        {
            var categories = await _product.ExtractCategories();

            Assert.IsNotNull(categories, "Expected categories list to not be null!");
            Assert.IsTrue(categories.Count > 0, "Expected at least one category in the list!");
        }
    }
}
