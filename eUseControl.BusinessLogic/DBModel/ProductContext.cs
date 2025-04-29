using System.Data.Entity;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.DBModel
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<ProductDbTable> Products { get; set; }
    }
}
