using System.Data.Entity;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.BusinessLogic.DBModel
{
    public class CategoryContext : DbContext
    {
        public CategoryContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<CategoryDbTable> ProductCategories { get; set; }
    }
}
