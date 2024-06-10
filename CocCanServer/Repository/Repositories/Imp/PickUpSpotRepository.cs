using Repository.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Repository.repositories.imp
{
    public class PickUpSpotRepository : IPickUpSpotRepository
    {
        private readonly CocCanDBContext _dataContext;

        public PickUpSpotRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<bool> CreatePickUpSpotAsync(PickUpSpot pickUpSpot)
        {
            await _dataContext.PickUpSpots.AddAsync(pickUpSpot);
            return await Save();
        }

        public async Task<ICollection<PickUpSpot>> GetAllPickUpSpotsAsync()
        {
            IQueryable<PickUpSpot> _pickUpSpots =
               _dataContext.PickUpSpots.Where(s => s.Status == 1);

            return await _pickUpSpots
                .ToListAsync();
        }

        public async Task<ICollection<PickUpSpot>>
            GetAllPickUpSpotsWithStatusAsync(string search, int from, int to, string filter, string orderBy, bool ascending)
        {
            IQueryable<PickUpSpot> _pickUpSpots =
                _dataContext.PickUpSpots.Where(s => s.Status == 1);

            return await _pickUpSpots
                .ToListAsync();
        }

        public async Task<PickUpSpot> GetPickUpSpotByGUIDAsync(Guid id)
        {
            return await _dataContext.PickUpSpots
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeletePickUpSpotAsync(Guid id)
        {
            var _existingPickUpSpot = await GetPickUpSpotByGUIDAsync(id);

            if (_existingPickUpSpot != null)
            {
                _existingPickUpSpot.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdatePickUpSpotAsync(PickUpSpot PickUpSpot)
        {
            _dataContext.PickUpSpots.Update(PickUpSpot);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
