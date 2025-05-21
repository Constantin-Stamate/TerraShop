namespace eUseControl.Web.Models.Cart
{
    public class CartCompact
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ProductQuantity { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductImageUrl { get; set; }

        public int SelectedQuantity { get; set; }

        public decimal Subtotal { get; set; }
    }
}