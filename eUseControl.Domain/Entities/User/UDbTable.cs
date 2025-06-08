using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(30)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "LastLogin")]
        public DateTime? LastLogin { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "RegistrationDateTime")]
        public DateTime RegistrationDateTime { get; set; }

        [StringLength(30)]
        [Display(Name = "LastIp")]
        public string LastIp { get; set; }

        [StringLength(30)]
        [Display(Name = "RegistrationIp")]
        public string RegistrationIp { get; set; }

        [EnumDataType(typeof(URole))]
        [Display(Name = "Level")]
        public URole? Level { get; set; }
    }
}
