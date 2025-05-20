using System.Data.Entity;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.DBModel
{
    public class CartContext : DbContext
    {
        public CartContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<CartDbTable> CartItems { get; set; }
    }
}
