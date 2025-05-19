using System;

namespace eUseControl.Domain.Entities.Product
{
    public class ProductSummary
    {
        public int Id { get; set; }

        public string ProductCategory {  get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public DateTime ProductPostDate { get; set; }

        public string ProductRegion { get; set; }

        public int ProductQuantity { get; set; }
    }
}
