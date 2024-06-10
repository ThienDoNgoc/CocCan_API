using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Location
{
    public class CreateLocationDTO
    {
        [Required(ErrorMessage = "[Name] field is required!")]
        [MaxLength(200, ErrorMessage = "[Name] field is 200 characters max length!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "[Address] field is required!")]
        [MaxLength(200, ErrorMessage = "[Address] field is 200 characters max length!")]
        public string Address { get; set; }
    }
}
