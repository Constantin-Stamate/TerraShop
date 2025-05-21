using System.Data.Entity;
using eUseControl.Domain.Entities.Cart;

namespace eUseControl.BusinessLogic.DBModel
{
    public class CouponContext : DbContext
    {
        public CouponContext() : base("name=eUseControl")
        {
        }

        public virtual DbSet<Coupon> DiscountCoupons { get; set; }
    }
}
