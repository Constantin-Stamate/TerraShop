namespace eUseControl.Domain.Entities.Product
{
    public class ProductMinimal
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }
    }
}
