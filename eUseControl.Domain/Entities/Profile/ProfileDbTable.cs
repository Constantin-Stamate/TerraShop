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
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastProfileUpdate { get; set; }

        [Required]
        [StringLength(50)]
        public string ProfileImageUrl { get; set; }
    }
}
