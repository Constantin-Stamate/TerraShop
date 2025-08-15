using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.Subscription
{
    [Table("Subscribers")]
    public class SubscriptionDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(30)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "SubscriptionDate")]
        public DateTime SubscriptionDate { get; set; }
    }
}
