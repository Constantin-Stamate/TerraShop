using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ProductName")]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "ProductAddress")]
        [StringLength(100)]
        public string ProductAddress { get; set; }

        [Required]
        [Display(Name = "ProductQuantity")]
        [Range(0, int.MaxValue)]
        public int ProductQuantity { get; set; }

        [Required]
        [Display(Name = "ProductQuality")]
        [StringLength(100)]
        public string ProductQuality { get; set; }

        [Required]
        [Display(Name = "ProductPrice")]
        [Range(0, double.MaxValue)]
        public decimal ProductPrice { get; set; }

        [Required]
        [Display(Name = "ProductRegion")]
        [StringLength(100)]
        public string ProductRegion { get; set; }

        [Display(Name = "ProductImageUrl")]
        [StringLength(200)]
        public string ProductImageUrl { get; set; }

        [Required]
        [Display(Name = "ProductDescription")]
        [StringLength(500)]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "ProductCategory")]
        public string ProductCategory { get; set; }

        [Display(Name = "ProductPostDate")]
        [DataType(DataType.Date)]
        public DateTime ProductPostDate { get; set; }

        [Display(Name = "ProductRating")]
        [Range(0, int.MaxValue)]
        public int ProductRating { get; set; }
    }
}