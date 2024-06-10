using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.OrderDetail
{
    public class UpdateOrderDetailDTO
    {
        [Required(AllowEmptyStrings = true)]
        public int Quantity { get; set; }

        [Required(AllowEmptyStrings = true)]
        public Guid MenuDetailId { get; set; }

        [Required(AllowEmptyStrings = true)]
        public Guid OrderId { get; set; }
        public decimal SinglePrice { get; set; }
    }
}
