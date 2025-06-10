using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.Contact
{
    public class ContactCompact
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Message")]
        [StringLength(500)]
        public string Message { get; set; }
    }
}