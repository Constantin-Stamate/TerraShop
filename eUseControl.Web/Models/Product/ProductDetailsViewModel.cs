using System.Collections.Generic;
using eUseControl.Web.Models.Profile;
using eUseControl.Web.Models.User;
using eUseControl.Web.Models.Review;

namespace eUseControl.Web.Models.Product
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }

        public UserCompact UserCompact { get; set; }

        public ReviewCompact ReviewCompact { get; set; }

        public Dictionary<ReviewCompact, ProfileMini> Reviews { get; set; }
    }
}