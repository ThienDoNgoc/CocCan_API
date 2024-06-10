using CocCanService.DTOs.Location;
using CocCanService.DTOs.Store;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface ILocationService
    {
        Task<ServiceResponse<List<DTOs.Location.LocationDTO>>> GetAllLocationsAsync();
        Task<ServiceResponse<DTOs.Location.LocationDTO>> CreateLocationAsync(CreateLocationDTO createLocationDTO);
        Task<ServiceResponse<DTOs.Location.LocationDTO>> UpdateLocationAsync(LocationDTO locationDTO);
        Task<ServiceResponse<DTOs.Location.LocationDTO>> GetLocationByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteLocationAsync(Guid id);
    }
}
