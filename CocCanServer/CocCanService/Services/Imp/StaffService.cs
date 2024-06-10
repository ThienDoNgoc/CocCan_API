using AutoMapper;
using CocCanService.DTOs;
using CocCanService.DTOs.Customer;
using CocCanService.DTOs.Enum;
using CocCanService.DTOs.Session;
using CocCanService.DTOs.Staff;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepo;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSetting;

        public StaffService(IStaffRepository staffRepo, IMapper mapper, IOptionsMonitor<AppSetting> appSetting)
        {
            _staffRepo = staffRepo;
            _mapper = mapper;
            _appSetting = appSetting.CurrentValue;
        }

        public async Task<ServiceResponse<TokenStaff>> CheckStaffLoginsAsync(string Email, string Password)
        {
            ServiceResponse<TokenStaff> _response = new();
            try
            {
                var _Staff = await _staffRepo.CheckStaffLoginsAsync(Email, Password);
                if (_Staff == null)
                {
                    _response.Status = false;
                    _response.Title = "Not Found!";
                    return _response;
                }
                else
                {
                    var tokenStaff = new TokenStaff() 
                    { 
                        staffDTO = _mapper.Map<StaffDTO>(_Staff),
                        token = GenerateToken(_Staff)
                    };
                    _response.Status = true;
                    _response.Title = "OK";
                    _response.Data= tokenStaff;
                }
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Data = null;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string>() { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<StaffDTO>> CreateStaffAsync(CreateStaffDTO createStaffDTO)
        {
            ServiceResponse<StaffDTO> _response = new();
            try
            {
                var _newStaff = _mapper.Map<Staff>(createStaffDTO);
                if (!await _staffRepo.CreateStaffAsync(_newStaff))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<StaffDTO>(_newStaff);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Data = null;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string>() { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<List<StaffDTO>>> GetAllStaffsAsync()
        {
            ServiceResponse<List<StaffDTO>> _response = new();
            try
            {
                var _StaffList = await _staffRepo.GetAllStaffsAsync();

                var _StaffListDTO = new List<StaffDTO>();

                foreach (var item in _StaffList)
                {
                    _StaffListDTO.Add(_mapper.Map<StaffDTO>(item));
                }

                _response.Status = true;
                _response.Data = _StaffListDTO;
                _response.Title = "OK";
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Data = null;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string>() { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<string>> SoftDeleteStaffAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingStaff = await _staffRepo.GetStaffAsync(id);
                if (_existingStaff == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _staffRepo.SoftDeleteStaffAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "SoftDeleted";
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Data = null;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string>() { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<StaffDTO>> UpdateStaffAsync(StaffDTO staffDTO)
        {
            ServiceResponse<StaffDTO> _response = new();
            try
            {
                var _existingStaff = await _staffRepo.GetStaffAsync(staffDTO.Id)
                    ;
                if (_existingStaff == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                _existingStaff.Fullname = staffDTO.Fullname;
                _existingStaff.Phone = staffDTO.Phone;

                if (!await _staffRepo.UpdateStaffAsync(_existingStaff))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "Updated";
                _response.Data = _mapper.Map<StaffDTO>(_existingStaff);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Data = null;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string>() { Convert.ToString(ex.Message) };
            }
            return _response;
        }
        public async Task<ServiceResponse<StaffDTO>> GetStaffByGUIDAsync(Guid id)
        {
            ServiceResponse<StaffDTO> _response = new();

            try
            {

                var _Staff = await _staffRepo.GetStaffAsync(id);

                if (_Staff == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _StaffDto = _mapper.Map<StaffDTO>(_Staff);

                _response.Status = true;
                _response.Title = "Got Staff";
                _response.Data = _StaffDto;


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

        private string GenerateToken(Staff staff)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, ((RoleType) Enum.Parse(typeof(RoleType),  staff.Role.ToString())).ToString()),
                    new Claim(ClaimTypes.Role, "Staff"),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
