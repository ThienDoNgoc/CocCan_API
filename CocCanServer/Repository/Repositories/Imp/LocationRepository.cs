using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Repository.repositories.imp
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CocCanDBContext _dataContext;

        public LocationRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateLocationAsync(Location location)
        {
            await _dataContext.Locations.AddAsync(location);
            return await Save();
        }

        public async Task<ICollection<Location>> GetAllLocationsWithStatusAsync()
        {
            IQueryable<Location> _locations =
                _dataContext.Locations.Where(s => s.Status == 1);

            return await _locations
                .ToListAsync();
        }

        public async Task<Location> GetLocationByGUIDAsync(Guid id)
        {
            return await _dataContext.Locations
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteLocationAsync(Guid id)
        {
            var _existingLocation = await GetLocationByGUIDAsync(id);

            if (_existingLocation != null)
            {
                _existingLocation.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            _dataContext.Locations.Update(location);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
