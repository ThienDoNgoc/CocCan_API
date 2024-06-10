using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.MenuDetail
{
    public class CreateMenuDetailDTO
    {
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "[MenuId] field is required!")]
        public Guid MenuId { get; set; }

    }
}
