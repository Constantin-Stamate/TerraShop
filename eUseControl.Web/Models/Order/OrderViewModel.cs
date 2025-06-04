using System.Collections.Generic;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Models.Order
{
    public class OrderViewModel
    {
        public OrderCompact Order { get; set; }

        public List<CartCompact> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string CouponCode { get; set; }
    }
}