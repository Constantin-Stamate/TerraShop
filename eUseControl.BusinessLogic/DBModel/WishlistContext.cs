using System.Data.Entity;
using eUseControl.Domain.Entities.Wishlist;

namespace eUseControl.BusinessLogic.DBModel
{
    public class WishlistContext : DbContext
    {
        public WishlistContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<WishlistDbTable> WishlistProducts { get; set; }
    }
}
