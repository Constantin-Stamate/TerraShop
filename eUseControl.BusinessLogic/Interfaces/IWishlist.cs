using System.Collections.Generic;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Wishlist;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IWishlist
    {
        WishlistResp AddProductToWishlist(int userId, int productId);

        List<ProductLite> GetAllWishlistProducts(int userId);

        int GetWishlistCountByUserId(int userId);

        WishlistResp RemoveProductFromWishlist(int productId, int userId);

        List<int> GetWishlistProductIds(int userId);
    }
}
