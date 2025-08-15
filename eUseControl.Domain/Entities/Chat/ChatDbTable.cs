using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.Chat
{
    [Table("ChatMessages")]
    public class ChatDbTable
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
        [Display(Name = "Prompt")]
        [Column(TypeName = "text")]
        public string Prompt { get; set; }

        [Required]
        [Display(Name = "Message")]
        [Column(TypeName = "text")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ResponseDate")]
        public DateTime ResponseDate { get; set; }
    }
}
