using System.Collections.Generic;

namespace eUseControl.Web.Models.Product
{
    public class ProductCatalogViewModel
    {
        public int? CategoryId { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public string SortOption {  get; set; }

        public int MaxPrice { get; set; }

        public string SearchQuery { get; set; }

        public string Country { get; set; }

        public IEnumerable<ProductMini> Products { get; set; }

        public Dictionary<ProductCategory, int> Categories { get; set; }
    }
}