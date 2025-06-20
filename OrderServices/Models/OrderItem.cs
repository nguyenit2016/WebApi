using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderServices.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }


        // Khóa ngoại tới bảng Order
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        // Khóa ngoại tới bảng Product
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
