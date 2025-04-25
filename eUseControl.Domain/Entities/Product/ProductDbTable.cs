using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace eUseControl.Domain.Entities.Product
{
    [Table("Products")]
    public class ProductDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductAddress { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductQuantity { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductQuality { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductRegion { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductImageUrl { get; set; }

        [Required]
        [StringLength(500)]
        public string ProductDescription { get; set; }

        [DataType(DataType.Date)]
        public DateTime ProductPostDate { get; set; }

        [EnumDataType(typeof(ProductStatus))]
        public ProductStatus ProductStatus { get; set; }
    }
}
