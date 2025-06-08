using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Entities.Product;

namespace eUseControl.Domain.Entities.Review
{
    [Table("ProductReviews")]
    public class ReviewDbTable
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
        [Display(Name = "ReviewPostDate")]
        public DateTime ReviewPostDate { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Review")]
        public string Review { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Rating")]
        public int Rating { get; set; }
    }
}
