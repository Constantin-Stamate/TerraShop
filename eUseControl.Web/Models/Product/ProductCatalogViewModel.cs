using System.Collections.Generic;

namespace eUseControl.Web.Models.Product
{
    public class ProductCatalogViewModel
    {
        public int? CategoryId { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<ProductMini> Products { get; set; }

        public Dictionary<ProductCategory, int> Categories { get; set; }
    }
}