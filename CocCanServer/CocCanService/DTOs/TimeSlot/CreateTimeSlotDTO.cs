using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.TimeSlot
{
    public class CreateTimeSlotDTO
    {
        [Required(ErrorMessage = "[StartTime] field is required!")]
        public string? StartTime { get; set; }

        [Required(ErrorMessage = "[EndTime] field is required!")]
        public string? EndTime { get; set; }
    }
}
