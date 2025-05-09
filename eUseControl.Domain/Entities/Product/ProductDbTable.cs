using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Product
{
    [Table("Products")]
    public class ProductDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "ProductName")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "ProductAddress")]
        public string ProductAddress { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "ProductQuantity")]
        public int ProductQuantity { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "ProductQuality")]
        public string ProductQuality { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "ProductPrice")]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "ProductRegion")]
        public string ProductRegion { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "ProductImageUrl")]
        public string ProductImageUrl { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "ProductDescription")]
        public string ProductDescription { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ProductPostDate")]
        public DateTime ProductPostDate { get; set; }

        [EnumDataType(typeof(ProductStatus))]
        [Display(Name = "ProductStatus")]
        public ProductStatus ProductStatus { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "ProductRating")]
        public int ProductRating { get; set; }
    }
}
