using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.Review
{
    [Table("Reviews")]
    public class ReviewDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

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
