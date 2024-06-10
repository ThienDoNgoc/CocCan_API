using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.DTOs.TimeSlot
{
    public class TimeSlotDTO
    {
        [Required(ErrorMessage = "[Id] field is required!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "[StartTime] field is required!")]
        public String? StartTime { get; set; }

        [Required(ErrorMessage = "[EndTime] field is required!")]
        public String? EndTime { get; set; }
    }
}
