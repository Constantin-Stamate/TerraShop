using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.Domain.Entities.Wishlist
{
    [Table("WishlistProducts")]
    public class WishlistDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UDbTable User { get; set; }

        [Required]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductDbTable Product { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "AddedDate")]
        public DateTime AddedDate { get; set; }
    }
}
