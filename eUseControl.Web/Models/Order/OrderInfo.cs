using eUseControl.Domain.Enums;
using System;

namespace eUseControl.Web.Models.Order
{
    public class OrderInfo
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderImageUrl { get; set; }
    }
}