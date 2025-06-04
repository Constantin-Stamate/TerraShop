using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Entities.Order;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Payment
{
    [Table("UserTransactions")]
    public class TransactionDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "OrderId")]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderDbTable Order { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UDbTable User { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(TransactionStatus))]
        [Display(Name = "TransactionStatus")]
        public TransactionStatus TransactionStatus { get; set; }
    }
}
