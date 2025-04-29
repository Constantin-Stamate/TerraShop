using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class ProductCompact
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "ProductDescription")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "ProductPrice")]
        public decimal ProductPrice { get; set; }

        [Required]
        [Display(Name = "ProductImageUrl")]
        public string ProductImageUrl { get; set; }
    }
}