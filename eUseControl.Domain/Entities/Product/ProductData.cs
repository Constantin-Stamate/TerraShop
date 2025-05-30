﻿using System;

namespace eUseControl.Domain.Entities.Product
{
    public class ProductData
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ProductName { get; set; }

        public string ProductAddress { get; set; }

        public int ProductQuantity { get; set; }

        public string ProductQuality { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductRegion { get; set; }

        public string ProductImageUrl { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public DateTime ProductPostDate { get; set; }

        public int ProductRating { get; set; }
    }
}
