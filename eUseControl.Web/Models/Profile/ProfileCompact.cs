using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.Profile
{
    public class ProfileCompact
    {
        public int Id { get; set; }

        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [Display(Name = "ProfileImageUrl")]
        [StringLength(30)]
        public string ProfileImageUrl { get; set; }
    }
}