using System;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.Review
{
    public class ReviewCompact
    {
        public int Id { get; set; }

        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [StringLength(30)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [StringLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Review")]
        public string Review { get; set; }

        [Required]
        [Range(0, 5)]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ReviewPostDate")]
        public DateTime ReviewPostDate { get; set; }
    }
}