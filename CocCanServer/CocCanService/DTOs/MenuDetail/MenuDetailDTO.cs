using CocCanService.DTOs.Product;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.MenuDetail
{
    public class MenuDetailDTO
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public Guid MenuId { get; set; }
        public ProductDTO Product { get; set; }
    }
}
