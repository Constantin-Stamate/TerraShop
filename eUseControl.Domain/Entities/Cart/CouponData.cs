using System;

namespace eUseControl.Domain.Entities.Cart
{
    public class CouponData
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public int DiscountPercent { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsActive { get; set; }
    }
}
