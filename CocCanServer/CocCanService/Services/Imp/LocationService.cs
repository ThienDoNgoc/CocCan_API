using AutoMapper;
using CocCanService.DTOs.Category;
using CocCanService.DTOs.Location;
using CocCanService.DTOs.Store;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class LocationService:ILocationService
    {
        private readonly ILocationRepository _locationRepo;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepo, IMapper mapper)
        {
            this._locationRepo = locationRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<LocationDTO>> CreateLocationAsync(CreateLocationDTO createLocationDTO)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var _newLocation = _mapper.Map<Location>(createLocationDTO);

                if (!await _locationRepo.CreateLocationAsync(_newLocation))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<LocationDTO>(_newLocation);
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

        public async Task<ServiceResponse<List<LocationDTO>>> GetAllLocationsAsync()
        {
            ServiceResponse<List<LocationDTO>> _response = new();
            try
            {
                var _LocationList = await _locationRepo.GetAllLocationsWithStatusAsync();

                var _LocationListDTO = new List<LocationDTO>();

                foreach (var item in _LocationList)
                {
                    _LocationListDTO.Add(_mapper.Map<LocationDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all stores";
                _response.Data = _LocationListDTO;
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

        public async Task<ServiceResponse<LocationDTO>> GetLocationByIdAsync(Guid id)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var _LocationList = await _locationRepo.GetLocationByGUIDAsync(id);

                if (_LocationList == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _LocationDto = _mapper.Map<LocationDTO>(_LocationList);

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _LocationDto;

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

        public async Task<ServiceResponse<string>> SoftDeleteLocationAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingLocation = await _locationRepo.GetLocationByGUIDAsync(id);

                if (_existingLocation == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _locationRepo.SoftDeleteLocationAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted store";
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

        public async Task<ServiceResponse<LocationDTO>> UpdateLocationAsync(LocationDTO locationDTO)
        {
            ServiceResponse<LocationDTO> _response = new();
            try
            {
                var _existingLocation = await _locationRepo.GetLocationByGUIDAsync(locationDTO.Id);

                if (_existingLocation == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingLocation = _mapper.Map<Location>(locationDTO);

                if (!await _locationRepo.UpdateLocationAsync(_existingLocation))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated store";
                _response.Data = _mapper.Map<LocationDTO>(_existingLocation);
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
