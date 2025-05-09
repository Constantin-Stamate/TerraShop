using eUseControl.Domain.Enums;

namespace eUseControl.Web.Models.Product
{
    public class ProductCompact
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public ProductStatus ProductStatus { get; set; }
    }
}