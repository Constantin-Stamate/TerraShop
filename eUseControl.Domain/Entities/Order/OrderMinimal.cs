using System;

namespace eUseControl.Domain.Entities.Order
{
    public class OrderMinimal
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
