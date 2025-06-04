namespace eUseControl.Domain.Entities.Order
{
    public class OrderData
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        public string PaymentMethod { get; set; }

        public decimal TotalPrice { get; set; }

        public string CouponCode { get; set; }
    }
}
