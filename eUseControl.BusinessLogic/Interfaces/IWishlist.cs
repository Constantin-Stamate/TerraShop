using System.Collections.Generic;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IWishlist
    {
        void AddProductToWishlist(int userId, int productId);

        List<ProductLite> GetAllWishlistProducts(int userId);

        int GetWishlistCountByUserId(int userId);

        void RemoveProductFromWishlist(int productId, int userId);

        List<int> GetWishlistProductIds(int userId);
    }
}
