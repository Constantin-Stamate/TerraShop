using System;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Order
{
    public class OrderLite
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderImageUrl { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }
    }
}
