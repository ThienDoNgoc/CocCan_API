using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Store
{
    public class UpdateStoreDTO
    {
        [Required(AllowEmptyStrings = true)]
        [MaxLength(100, ErrorMessage = "[Name] field in StoreDTO is 100 characters max length!")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200, ErrorMessage = "[Image] field in StoreDTO is 200 characters max length!")]
        public string Image { get; set; }

        [Required(ErrorMessage = "[Address] field is required!")]
        [MaxLength(200, ErrorMessage = "[Address] field is 200 characters max length!")]
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
