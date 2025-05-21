using System.Collections.Generic;

namespace eUseControl.Web.Models.Cart
{
    public class CartViewModel
    {
        public List<CartCompact> Items { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal InitialPrice { get; set; }
    }
}