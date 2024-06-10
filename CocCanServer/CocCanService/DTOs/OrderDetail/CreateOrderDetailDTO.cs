using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.OrderDetail
{
    public class CreateOrderDetailDTO
    {
        [Required(ErrorMessage = "[Quantity] field is required!")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "[ProductId] field is required!")]
        public Guid MenuDetailId { get; set; }
        public decimal SinglePrice { get; set; }
    }
}
