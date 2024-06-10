using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Category
{
    public class UpdateCategoryDTO
    {
        [Required(AllowEmptyStrings = true)]
        [MaxLength(100, ErrorMessage = "[Name] field is 100 characters max length!")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Image { get; set; }
    }
}
