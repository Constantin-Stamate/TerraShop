using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eUseControl.Domain.Entities.Profile
{
    [Table("UserProfiles")]
    public class ProfileDbTable
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
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        [StringLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "LastProfileUpdate")]
        [DataType(DataType.Date)]
        public DateTime LastProfileUpdate { get; set; }

        [Display(Name = "ProfileImageUrl")]
        [StringLength(50)]
        public string ProfileImageUrl { get; set; }
    }
}
