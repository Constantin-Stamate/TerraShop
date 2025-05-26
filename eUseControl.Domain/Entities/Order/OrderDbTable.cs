using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eUseControl.Domain.Enums;

namespace eUseControl.Domain.Entities.Order
{
    [Table("CustomerOrders")]
    public class OrderDbTable
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
        [StringLength(30)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(70)]
        [Display(Name = "DeliveryAddress")]
        public string DeliveryAddress { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "PaymentMethod")]
        public string PaymentMethod { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        [Display(Name = "OrderStatus")]
        public OrderStatus OrderStatus { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Display(Name = "TotalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
