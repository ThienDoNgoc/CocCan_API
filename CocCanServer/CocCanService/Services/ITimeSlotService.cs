using CocCanService.DTOs.TimeSlot;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface ITimeSlotService
    {
        Task<ServiceResponse<List<TimeSlotDTO>>> GetAllTimeSlotsAsync();
        Task<ServiceResponse<TimeSlotDTO>> CreateTimeSlotAsync(CreateTimeSlotDTO createTimeSlotDTO);
        Task<ServiceResponse<TimeSlotDTO>> UpdateTimeSlotAsync(TimeSlotDTO timeSlotDTO);
        Task<ServiceResponse<string>> SoftDeleteTimeSlotAsync(Guid id);
        Task<ServiceResponse<bool>> HardDeleteTimeSlotAsync(TimeSlotDTO timeSlotDTO);
        Task<ServiceResponse<TimeSlotDTO>> GetTimeSlotByGUIDAsync(Guid id);
    }
}
