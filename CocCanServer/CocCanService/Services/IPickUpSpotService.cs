using CocCanService.DTOs.PickUpSpot;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IPickUpSpotService
    {
        Task<ServiceResponse<List<DTOs.PickUpSpot.PickUpSpotDTO>>> GetAllPickUpSpotsAsync();
        Task<ServiceResponse<DTOs.PickUpSpot.PickUpSpotDTO>> CreatePickUpSpotAsync(CreatePickUpSpotDTO createPickUpSpotDTO);
        Task<ServiceResponse<DTOs.PickUpSpot.PickUpSpotDTO>> UpdatePickUpSpotAsync(PickUpSpotDTO pickUpSpotDTO);
        Task<ServiceResponse<DTOs.PickUpSpot.PickUpSpotDTO>> GetPickUpSpotByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeletePickUpSpotAsync(Guid id);
    }
}
