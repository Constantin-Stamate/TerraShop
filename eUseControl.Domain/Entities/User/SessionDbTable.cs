using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("UserSessions")]
    public class SessionDbTable
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
        [StringLength(500)]
        [Display(Name = "CookieString")]
        public string CookieString { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "ExpireTime")]
        public DateTime ExpireTime { get; set; }
    }
}
