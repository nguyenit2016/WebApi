using System.ComponentModel.DataAnnotations;

namespace OrderServices.ViewModels
{
    public class OrderFilterRequest
    {
        public int? CustomerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
    }
}
