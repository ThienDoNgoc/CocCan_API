using AutoMapper;
using CocCanService.DTOs.Store;
using CocCanService.DTOs.TimeSlot;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository _TimeSlotRepo;
        private readonly IMapper _mapper;

        public TimeSlotService(ITimeSlotRepository TimeSlotRepo, IMapper mapper)
        {
            _TimeSlotRepo = TimeSlotRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<TimeSlotDTO>> CreateTimeSlotAsync(CreateTimeSlotDTO createTimeSlotDTO)
        {
            ServiceResponse<TimeSlotDTO> _response = new();
            try
            {
                var _newTimeSlot = _mapper.Map<TimeSlot>(createTimeSlotDTO);
                if (!await _TimeSlotRepo.CreateTimeSlotAsync(_newTimeSlot))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Category Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<TimeSlotDTO>(_newTimeSlot);
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

        public async Task<ServiceResponse<List<TimeSlotDTO>>> GetAllTimeSlotsAsync()
        {
            ServiceResponse<List<TimeSlotDTO>> _response = new();
            try
            {
                var _TimeSlotList = await _TimeSlotRepo.GetAllTimeSlotWithStatusAsync();

                var _TimeSlotListDTO = new List<TimeSlotDTO>();

                foreach (var item in _TimeSlotList)
                {
                    _TimeSlotListDTO.Add(_mapper.Map<TimeSlotDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all TimeSlots";
                _response.Data = _TimeSlotListDTO;
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

        public async Task<ServiceResponse<TimeSlotDTO>> GetTimeSlotByGUIDAsync(Guid id)
        {
            ServiceResponse<TimeSlotDTO> _response = new();

            try
            {

                var _TimeSlot = await _TimeSlotRepo.GetTimeSlotByGUIDAsync(id);

                if (_TimeSlot == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _TimeSlotDto = _mapper.Map<TimeSlotDTO>(_TimeSlot);

                _response.Status = true;
                _response.Title = "Got TimeSlot";
                _response.Data = _TimeSlotDto;


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

        public Task<ServiceResponse<bool>> HardDeleteTimeSlotAsync(TimeSlotDTO TimeSlotDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> SoftDeleteTimeSlotAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingTimeSlot = await _TimeSlotRepo.GetTimeSlotByGUIDAsync(id);
                if (_existingTimeSlot == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _TimeSlotRepo.SoftDeleteTimeSlotAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete category!");
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "Deleted TimeSlot";
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

        public async Task<ServiceResponse<TimeSlotDTO>> UpdateTimeSlotAsync(TimeSlotDTO TimeSlotDTO)
        {
            ServiceResponse<TimeSlotDTO> _response = new();
            try
            {
                var _existingTimeSlot = await _TimeSlotRepo.GetTimeSlotByGUIDAsync(TimeSlotDTO.Id)
                    ;
                if (_existingTimeSlot == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingTimeSlot = _mapper.Map<TimeSlot>(TimeSlotDTO);

                if (!await _TimeSlotRepo.UpdateTimeSlotAsync(_existingTimeSlot))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated category";
                _response.Data = _mapper.Map<TimeSlotDTO>(_existingTimeSlot);
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
