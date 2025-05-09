using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities
{
    [Table("Sessions")]
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "CookieString")]
        public string CookieString { get; set; }

        [Required]
        [Display(Name = "ExpireTime")]
        public DateTime ExpireTime { get; set; }
    }
}
