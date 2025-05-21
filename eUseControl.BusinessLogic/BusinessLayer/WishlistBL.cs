using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Wishlist;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class WishlistBL : UserApi, IWishlist
    {
        public WishlistResp AddProductToWishlist(int userId, int productId)
        {
            return AddProductToWishlistAction(userId, productId);
        }

        public List<ProductLite> GetAllWishlistProducts(int userId)
        {
            return GetAllWishlistProductsAction(userId);
        }

        public int GetWishlistCountByUserId(int userId)
        {
            return GetWishlistCountByUserIdAction(userId);
        }

        public WishlistResp RemoveProductFromWishlist(int productId, int userId)
        {
            return RemoveProductFromWishlistAction(productId, userId);
        }

        public List<int> GetWishlistProductIds(int userId)
        {
            return GetWishlistProductIdsAction(userId);
        }
    }
}
