using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.Session
{
    public class CreateSessionDTO
    {

        [Required(ErrorMessage = "[TimeSlotId] field is required!")]
        public Guid TimeSlotId { get; set; }

        [Required(ErrorMessage = "[LocationId] field is required!")]
        public Guid LocationId { get; set; }

        [Required(ErrorMessage = "[MenuId] field is required!")]
        public Guid MenuId { get; set; }
        public DateTime Date { get; set; }
        public int? SessionStatus { get; set; }
    }
}
