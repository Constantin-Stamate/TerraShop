using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUseControl.Domain.Entities.Cart
{
    [Table("CartItems")]
    public class CartDbTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "SelectedQuantity")]
        public int SelectedQuantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "AddedDate")]
        public DateTime AddedDate { get; set; }
    }
}
