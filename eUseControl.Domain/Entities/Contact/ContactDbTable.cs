using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Contact
{
    [Table("ContactRequests")]
    public class ContactDbTable
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

        [EnumDataType(typeof(RequestStatus))]
        [Display(Name = "RequestStatus")]
        public RequestStatus RequestStatus { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "RequestPostDate")]
        public DateTime RequestPostDate { get; set; }
    }
}
