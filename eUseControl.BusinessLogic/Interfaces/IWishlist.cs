using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Entities.Product;
using eUseControl.Domain.Entities.Wishlist;

namespace eUseControl.BusinessLogic.Interfaces
{
    public interface IWishlist
    {
        Task<WishlistResp> AddProductToWishlist(int userId, int productId);

        Task<List<ProductLite>> GetAllWishlistProducts(int userId);

        int GetWishlistCountByUserId(int userId);

        Task<WishlistResp> RemoveProductFromWishlist(int productId, int userId);

        Task<List<int>> GetWishlistProductIds(int userId);
    }
}
