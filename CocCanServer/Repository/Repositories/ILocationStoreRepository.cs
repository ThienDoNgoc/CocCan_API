using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface ILocationStoreRepository
    {
        Task<ICollection<LocationStore>>
            GetAllLocationStoresWithStatusAsync(string search, int from, int to, string filter, string orderBy, bool ascending);
        Task<bool> CreateLocationStoreAsync(LocationStore locationStore);
        Task<bool> UpdateLocationStoreAsync(LocationStore locationStore);
        Task<bool> SoftDeleteLocationStoreAsync(Guid id);
        Task<LocationStore> GetLocationStoreByGUIDAsync(Guid id);
    }
}
