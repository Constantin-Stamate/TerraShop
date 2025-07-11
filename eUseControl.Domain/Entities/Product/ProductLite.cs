using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Product
{
    public class ProductLite
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public int ProductQuantity { get; set; }

        public string ProductQuality { get; set; }

        public RecommendationStatus RecommendationStatus { get; set; }

        public string ProductCategory { get; set; }
    }
}
