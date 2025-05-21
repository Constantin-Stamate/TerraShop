using System.Collections.Generic;

namespace eUseControl.Web.Models.Product
{
    public class ProductNavigationViewModel
    {
        public Dictionary<ProductCategory, int> Categories { get; set; }

        public int WishlistCount { get; set; }

        public int CartCount { get; set; }
    }
}