using Repository.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Repository.repositories.imp
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly CocCanDBContext _dataContext;

        public TimeSlotRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateTimeSlotAsync(TimeSlot timeSlot)
        {
            await _dataContext.TimeSlots.AddAsync(timeSlot);
            return await Save();
        }

        public async Task<ICollection<TimeSlot>>
            GetAllTimeSlotWithStatusAsync()
        {
            IQueryable<TimeSlot> _timeSlots =
                _dataContext.TimeSlots.Where(s => s.Status == 1);

            return await _timeSlots
                .ToListAsync();
        }

        public async Task<TimeSlot> GetTimeSlotByGUIDAsync(Guid id)
        {
            return await _dataContext.TimeSlots
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteTimeSlotAsync(Guid id)
        {
            var _existingTimeSlot = await GetTimeSlotByGUIDAsync(id);

            if (_existingTimeSlot != null)
            {
                _existingTimeSlot.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateTimeSlotAsync(TimeSlot timeSlot)
        {
            _dataContext.TimeSlots.Update(timeSlot);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
