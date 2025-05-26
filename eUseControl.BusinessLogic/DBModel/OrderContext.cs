using System.Data.Entity;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.DBModel
{
    public class OrderContext : DbContext
    {
        public OrderContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<OrderDbTable> CustomerOrders { get; set; }
    }
}
