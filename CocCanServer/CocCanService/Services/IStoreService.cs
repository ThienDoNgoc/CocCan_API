using CocCanService.DTOs.Staff;
using CocCanService.DTOs.Store;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IStoreService
    {
        Task<ServiceResponse<List<StoreDTO>>> 
            GetAllStoresWithStatusAsync(string filter, string range, string sort);
        Task<ServiceResponse<StoreDTO>> CreateStoreAsync(CreateStoreDTO createStoreDTO);
        Task<ServiceResponse<StoreDTO>> UpdateStoreAsync(Guid id, UpdateStoreDTO UpdateStoreDTO);
        Task<ServiceResponse<StoreDTO>> GetStoreByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteStoreAsync(Guid id);
    }
}
