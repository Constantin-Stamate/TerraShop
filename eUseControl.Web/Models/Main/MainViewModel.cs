using System.Collections.Generic;
using eUseControl.Web.Models.Product;

namespace eUseControl.Web.Models.Main
{
    public class MainViewModel
    {
        public List<ReviewProfileData> ReviewsWithProfiles { get; set; }

        public Dictionary<string, List<ProductMini>> Products { get; set; }

        public List<int> WishlistProductIds { get; set; }
    }
}