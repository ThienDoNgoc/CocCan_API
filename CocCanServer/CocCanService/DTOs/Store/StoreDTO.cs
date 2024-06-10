using CocCanService.DTOs.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Store
{
    public class StoreDTO
    {
        [Required(ErrorMessage="[Id] field is required!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage="[Name] field is required!")]
        [MaxLength(100,ErrorMessage= "[Name] field is 100 characters max length!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "[Image] field is required!")]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Image { get; set; }

        [Required(ErrorMessage = "[Address] field is required!")]
        [MaxLength(200, ErrorMessage = "[Address] field is 200 characters max length!")]
        public string Address { get; set; }
        public string? Phone { get; set; }

        [Required(ErrorMessage = "[Products] field is required!")]
        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
