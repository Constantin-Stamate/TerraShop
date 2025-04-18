using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities
{
    [Table("Users")]
    public class UDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username cannot be longer than 30 characters")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password cannot be shorter than 8 characters")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [StringLength(30)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LastLogin { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDateTime { get; set; }

        [StringLength(30)]
        public string LastIp { get; set; }

        [StringLength(30)]
        public string RegistrationIp { get; set; }

        public URole? Level { get; set; }
    }
}
