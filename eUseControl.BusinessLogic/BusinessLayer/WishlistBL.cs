using System.Collections.Generic;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.BusinessLayer
{
    public class WishlistBL : UserApi, IWishlist
    {
        public void AddProductToWishlist(int userId, int productId)
        {
            AddProductToWishlistAction(userId, productId);
        }

        public List<ProductLite> GetAllWishlistProducts(int userId)
        {
            return GetAllWishlistProductsAction(userId);
        }

        public int GetWishlistCountByUserId(int userId)
        {
            return GetWishlistCountByUserIdAction(userId);
        }

        public void RemoveProductFromWishlist(int productId, int userId)
        {
            RemoveProductFromWishlistAction(productId, userId);
        }

        public List<int> GetWishlistProductIds(int userId)
        {
            return GetWishlistProductIdsAction(userId);
        }
    }
}
