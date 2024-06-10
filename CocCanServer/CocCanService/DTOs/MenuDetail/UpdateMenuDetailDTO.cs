using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.MenuDetail
{
    public class UpdateMenuDetailDTO
    {
        public decimal Price { get; set; }
        public Guid MenuId { get; set; }
        public Guid ProductId { get; set; }
    }
}
