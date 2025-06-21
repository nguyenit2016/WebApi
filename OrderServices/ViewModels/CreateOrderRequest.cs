using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderServices.ViewModels
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CreateOrderItem> OrderItems { get; set; } = new List<CreateOrderItem>();
    }

    public class CreateOrderItem
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal UnitPrice { get; set; }
    }
}
