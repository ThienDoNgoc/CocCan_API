using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Customer
{
    public class UpdateCustomerDTO
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings = true)]
        [MaxLength(100, ErrorMessage = "[Fullname] field is 100 characters max length!")]
        public string Fullname { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Image { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
