using AutoMapper;
using CocCanService.DTOs.PickUpSpot;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class PickUpSpotService:IPickUpSpotService
    {
        private readonly IPickUpSpotRepository _PickUpSpotRepo;
        private readonly IMapper _mapper;

        public PickUpSpotService(IPickUpSpotRepository PickUpSpotRepo, IMapper mapper)
        {
            this._PickUpSpotRepo = PickUpSpotRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<PickUpSpotDTO>> CreatePickUpSpotAsync(CreatePickUpSpotDTO createPickUpSpotDTO)
        {
            ServiceResponse<PickUpSpotDTO> _response = new();
            try
            {
                Repository.Entities.PickUpSpot _newPickUpSpot = new()
                {
                    Id = Guid.NewGuid(),
                    Fullname = createPickUpSpotDTO.Fullname,
                    Address = createPickUpSpotDTO.Address,
                    LocationId = createPickUpSpotDTO.LocationId,
                    Status = 1
                };

                if (!await _PickUpSpotRepo.CreatePickUpSpotAsync(_newPickUpSpot))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in PickUpSpot Repository when trying to create PickUpSpot!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<PickUpSpotDTO>(_newPickUpSpot);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<List<PickUpSpotDTO>>> GetAllPickUpSpotsAsync()
        {
            ServiceResponse<List<PickUpSpotDTO>> _response = new();
            try
            {
                var _PickUpSpotList = await _PickUpSpotRepo.GetAllPickUpSpotsAsync();

                var _PickUpSpotListDTO = new List<PickUpSpotDTO>();

                foreach (var item in _PickUpSpotList)
                {
                    _PickUpSpotListDTO.Add(_mapper.Map<PickUpSpotDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _PickUpSpotListDTO;
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<PickUpSpotDTO>> GetPickUpSpotByIdAsync(Guid id)
        {
            ServiceResponse<PickUpSpotDTO> _response = new();
            try
            {
                var _PickUpSpotList = await _PickUpSpotRepo.GetPickUpSpotByGUIDAsync(id);

                if (_PickUpSpotList == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _PickUpSpotDto = _mapper.Map<PickUpSpotDTO>(_PickUpSpotList);

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _PickUpSpotDto;

            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<string>> SoftDeletePickUpSpotAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingPickUpSpot = await _PickUpSpotRepo.GetPickUpSpotByGUIDAsync(id);

                if (_existingPickUpSpot == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _PickUpSpotRepo.SoftDeletePickUpSpotAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in PickUpSpot Repository when trying to delete PickUpSpot!");
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "Created";
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<PickUpSpotDTO>> UpdatePickUpSpotAsync(PickUpSpotDTO pickUpSpotDTO)
        {
            ServiceResponse<PickUpSpotDTO> _response = new();
            try
            {
                var _existingPickUpSpot = await _PickUpSpotRepo.GetPickUpSpotByGUIDAsync(pickUpSpotDTO.Id);

                if (_existingPickUpSpot == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }
                _existingPickUpSpot.Fullname = pickUpSpotDTO.Fullname;
                _existingPickUpSpot.Address = pickUpSpotDTO.Address;
                _existingPickUpSpot.LocationId = pickUpSpotDTO.LocationId;
                _existingPickUpSpot.Status = pickUpSpotDTO.Status;

                if (!await _PickUpSpotRepo.UpdatePickUpSpotAsync(_existingPickUpSpot))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in PickUpSpot Repository when trying to delete PickUpSpot!");
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<PickUpSpotDTO>(_existingPickUpSpot);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }
    }
}
