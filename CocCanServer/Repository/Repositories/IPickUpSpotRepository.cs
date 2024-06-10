using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface IPickUpSpotRepository
    {
        Task<ICollection<PickUpSpot>> GetAllPickUpSpotsAsync();
        Task<bool> CreatePickUpSpotAsync(PickUpSpot pickUpSpot);
        Task<bool> UpdatePickUpSpotAsync(PickUpSpot pickUpSpot);
        Task<bool> SoftDeletePickUpSpotAsync(Guid id);
        Task<PickUpSpot> GetPickUpSpotByGUIDAsync(Guid id);
    }
}
