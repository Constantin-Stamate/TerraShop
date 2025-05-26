using System.Collections.Generic;
using eUseControl.Web.Models.Cart;

namespace eUseControl.Web.Models.Order
{
    public class CheckoutViewModel
    {
        public OrderCompact Order { get; set; }

        public List<CartCompact> CartItems { get; set; }
    }
}