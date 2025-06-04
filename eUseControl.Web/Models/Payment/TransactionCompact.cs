using System.ComponentModel.DataAnnotations;

namespace eUseControl.Web.Models.Payment
{
    public class TransactionCompact
    {
        [Required]
        [Display(Name = "OrderId")]
        public int OrderId { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "CardNumber")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{2})\/(0[1-9]|1[0-2])$")]
        [Display(Name = "ExpiryDate")]
        public string ExpiryDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}$")]
        [Display(Name = "Cvv")]
        public string Cvv { get; set; }
    }
}