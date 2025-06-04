using System;

namespace eUseControl.Web.Models.Order
{
    public class OrderMini
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}