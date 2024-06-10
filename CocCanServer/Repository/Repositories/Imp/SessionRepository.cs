using Repository.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Repository.repositories.imp
{
    public class SessionRepository : ISessionRepository
    {
        private readonly CocCanDBContext _dataContext;

        public SessionRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateSessionAsync(Session session)
        {
            await _dataContext.Sessions.AddAsync(session);
            return await Save();
        }

        public async Task<ICollection<Session>>
            GetAllSessionsWithStatusAsync(Dictionary<string, List<string>> filter)
        {
            var date = DateTime.Today.AddHours(7);
            IQueryable<Session> _sessions =
                _dataContext.Sessions
                .Where(s => s.Status == 1)
                .OrderByDescending(s => s.Date);

            if (filter != null && filter.ContainsKey("timeslot") && filter.ContainsKey("location"))
            {
                _sessions = _dataContext.Sessions.Where(s => s.Date == date);
                _sessions = _sessions
                    .Join(_dataContext.TimeSlots.Where(ts => ts.Id.ToString() == filter["timeslot"][0]),
                    s => s.TimeSlotId,
                    ts => ts.Id,
                    (s, ts) => s)
                    .Join(_dataContext.Locations.Where(l => l.Id.ToString() == filter["location"][0]),
                    s => s.LocationId,
                    ts => ts.Id,
                    (s, ts) => s);

            }

            _sessions = _sessions
                .Include(s => s.Menu);

            return await _sessions
                .ToListAsync();
        }

        public async Task<Session> GetSessionByGUIDAsync(Guid id)
        {
            return await _dataContext.Sessions
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteSessionAsync(Guid id)
        {
            var _existingSession = await GetSessionByGUIDAsync(id);

            if (_existingSession != null)
            {
                _existingSession.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateSessionAsync(Session Session)
        {
            _dataContext.Sessions.Update(Session);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
