using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.User
{
    public class UserLogin
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string Password { get; set; }
    }
}