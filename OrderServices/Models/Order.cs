using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderServices.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }


        //Khóa ngoại tới bảng Customer
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public virtual IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
