using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Wishlist;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class WishlistBL : UserApi, IWishlist
    {
        public async Task<WishlistResp> AddProductToWishlist(int userId, int productId)
        {
            return await AddProductToWishlistAction(userId, productId);
        }

        public async Task<List<ProductLite>> GetAllWishlistProducts(int userId)
        {
            return await GetAllWishlistProductsAction(userId);
        }

        public int GetWishlistCountByUserId(int userId)
        {
            return GetWishlistCountByUserIdAction(userId);
        }

        public async Task<WishlistResp> RemoveProductFromWishlist(int productId, int userId)
        {
            return await RemoveProductFromWishlistAction(productId, userId);
        }

        public async Task<List<int>> GetWishlistProductIds(int userId)
        {
            return await GetWishlistProductIdsAction(userId);
        }
    }
}
