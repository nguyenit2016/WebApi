using System.ComponentModel.DataAnnotations;

namespace OrderServices.ViewModels
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm.")]
        public decimal Price { get; set; }
    }
}
