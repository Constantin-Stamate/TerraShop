using System.Threading.Tasks;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eUseControl.Tests.ServicesTests
{
    [TestClass]
    public class WishlistTests
    {
        private readonly IWishlist _wishlist;

        public WishlistTests()
        {
            var bl = new BusinessLogicManager();
            _wishlist = bl.GetWishlistBL();
        }

        [TestMethod]
        public async Task AddNullProduct()
        {
            int userId = 0;
            int productId = 0;

            var result = await _wishlist.AddProductToWishlist(userId, productId);

            Assert.IsFalse(result.Status, "Expected failure for null product!");
            Assert.AreEqual("An error occurred while adding the product to the wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task AddInvalidUser()
        {
            int userId = -1;
            int productId = 5;

            var result = await _wishlist.AddProductToWishlist(userId, productId);

            Assert.IsFalse(result.Status, "Expected failure for invalid user!");
            Assert.AreEqual("An error occurred while adding the product to the wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task AddInvalidProduct()
        {
            int userId = 1;
            int productId = -1;

            var result = await _wishlist.AddProductToWishlist(userId, productId);

            Assert.IsFalse(result.Status, "Expected failure for invalid product!");
            Assert.AreEqual("An error occurred while adding the product to the wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task AddProductSuccess()
        {
            int userId = 1;
            int productId = 1;

            var result = await _wishlist.AddProductToWishlist(userId, productId);

            Assert.IsTrue(result.Status, "Expected product to be added successfully!");
            Assert.AreEqual("Product added to wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task GetProductsInvalidUser()
        {
            int userId = -1;

            var result = await _wishlist.GetAllWishlistProducts(userId);

            Assert.IsNotNull(result, "Result should not be null!");
            Assert.AreEqual(0, result.Count, "Expected empty list for invalid user!");
        }

        [TestMethod]
        public async Task GetProductsNoWishlist()
        {
            int userId = 4;

            var result = await _wishlist.GetAllWishlistProducts(userId);

            Assert.IsNotNull(result, "Result should not be null!");
            Assert.AreEqual(0, result.Count, "Expected no products in wishlist!");
        }

        [TestMethod]
        public async Task GetProductsSuccess()
        {
            int userId = 1;

            var result = await _wishlist.GetAllWishlistProducts(userId);

            Assert.IsNotNull(result, "Result should not be null!");
            Assert.IsTrue(result.Count > 0, "Expected at least one product in wishlist!");
        }

        [TestMethod]
        public void WishlistCountInvalidUser()
        {
            int userId = -1;

            var result = _wishlist.GetWishlistCountByUserId(userId);

            Assert.AreEqual(0, result, "Expected count 0 for invalid user!");
        }

        [TestMethod]
        public void WishlistCountNoProducts()
        {
            int userId = 4;

            var result = _wishlist.GetWishlistCountByUserId(userId);

            Assert.AreEqual(0, result, "Expected count 0 for user with no wishlist!");
        }

        [TestMethod]
        public void GetWishlistCountSuccess()
        {
            int userId = 1;

            var count = _wishlist.GetWishlistCountByUserId(userId);

            Assert.IsTrue(count > 0, "Expected wishlist count to be greater than zero!");
        }

        [TestMethod]
        public async Task RemoveInvalidInput()
        {
            int productId = -1;
            int userId = -5;

            var result = await _wishlist.RemoveProductFromWishlist(productId, userId);

            Assert.IsFalse(result.Status, "Expected removal failure for invalid input!");
            Assert.AreEqual("Product not found in wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task RemoveProductSuccess()
        {
            int productId = 1;
            int userId = 1;

            var result = await _wishlist.RemoveProductFromWishlist(productId, userId);

            Assert.IsTrue(result.Status, "Expected successful removal!");
            Assert.AreEqual("Product removed from wishlist!", result.StatusMsg);
        }

        [TestMethod]
        public async Task GetProductIdsInvalidUser()
        {
            int userId = -1;

            var result = await _wishlist.GetWishlistProductIds(userId);

            Assert.IsNotNull(result, "Result should not be null!");
            Assert.AreEqual(0, result.Count, "Expected 0 product IDs for invalid user!");
        }

        [TestMethod]
        public async Task GetProductIdsSuccess()
        {
            int userId = 2;

            var result = await _wishlist.GetWishlistProductIds(userId);

            Assert.IsNotNull(result, "Result should not be null!");
            Assert.IsTrue(result.Count > 0, "Expected at least one product ID in wishlist!");
        }
    }
}
