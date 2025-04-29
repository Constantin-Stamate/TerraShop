using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "ProductAddress")]
        public string ProductAddress { get; set; }

        [Required]
        [Display(Name = "ProductQuantity")]
        public int ProductQuantity { get; set; }

        [Required]
        [Display(Name = "ProductQuality")]
        public string ProductQuality { get; set; }

        [Required]
        [Display(Name = "ProductPrice")]
        public decimal ProductPrice { get; set; }

        [Required]
        [Display(Name = "ProductRegion")]
        public string ProductRegion { get; set; }

        [Required]
        [Display(Name = "ProductImageUrl")]
        public string ProductImageUrl { get; set; }

        [Required]
        [Display(Name = "ProductDescription")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "ProductCategory")]
        public string ProductCategory { get; set; }
    }
}