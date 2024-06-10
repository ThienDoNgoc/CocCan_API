using CocCanService.DTOs.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.OrderDetail
{
    public class OrderDetailDTO
    {
        [Required(ErrorMessage = "[Id] field is required!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "[Quantity] field is required!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "[MenuDetailId] field is required!")]
        public Guid MenuDetailId { get; set; }

        [Required(ErrorMessage = "[OrderId] field is required!")]
        public Guid OrderId { get; set; }
        public decimal? SinglePrice { get; set; }
        public virtual ProductDTO Product { get; set; }
    }
}
