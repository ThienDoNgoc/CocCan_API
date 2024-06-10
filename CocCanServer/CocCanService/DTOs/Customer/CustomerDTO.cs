using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Customer
{
    public class CustomerDTO
    {
        [Required(ErrorMessage = "[Id] field is required!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "[Fullname] field is required!")]
        [MaxLength(100, ErrorMessage = "[Fullname] field is 100 characters max length!")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "[Image] field is required!")]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Image { get; set; }

        [Required(ErrorMessage = "[Email] field is required!")]
        [MaxLength(200, ErrorMessage = "[Image] field is 200 characters max length!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "[Phone] field is required!")]
        public string Phone { get; set; }
        public string Username { get; set; }
    }
}
