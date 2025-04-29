using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models
{
    public class ProfileCompact
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ProfileImageUrl")]
        public string ProfileImageUrl { get; set; }
    }
}