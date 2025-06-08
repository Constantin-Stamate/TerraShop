using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.Cart
{
    [Table("DiscountCoupons")]
    public class CouponDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "DiscountPercent")]
        public int DiscountPercent { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        [UIHint("Boolean")]
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
    }
}
