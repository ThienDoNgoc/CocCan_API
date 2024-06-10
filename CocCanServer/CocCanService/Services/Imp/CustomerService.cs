using AutoMapper;
using CocCanService.DTOs;
using CocCanService.DTOs.Customer;
using CocCanService.DTOs.Customer;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _CustomerRepo;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSetting;

        public CustomerService(ICustomerRepository CustomerRepo, IMapper mapper, IOptionsMonitor<AppSetting> appSetting)
        {
            this._CustomerRepo = CustomerRepo;
            this._mapper = mapper;
            _appSetting = appSetting.CurrentValue;
        }

        public async Task<ServiceResponse<CustomerDTO>> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var _newCustomer = _mapper.Map<Customer>(createCustomerDTO);

                if (!await _CustomerRepo.CreateCustomerAsync(_newCustomer))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Customer Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<CustomerDTO>(_newCustomer);
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

        public async Task<ServiceResponse<List<CustomerDTO>>> GetAllCustomersAsync()
        {
            ServiceResponse<List<CustomerDTO>> _response = new();
            try
            {
                var _CustomerList = await _CustomerRepo.GetAllCustomersWithStatusAsync();

                var _CustomerListDTO = new List<CustomerDTO>();

                foreach (var item in _CustomerList)
                {
                    _CustomerListDTO.Add(_mapper.Map<CustomerDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all Customers";
                _response.Data = _CustomerListDTO;
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

        public async Task<ServiceResponse<CustomerDTO>> GetCustomerByIdAsync(Guid id)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var _CustomerList = await _CustomerRepo.GetCustomerByGUIDAsync(id);

                if (_CustomerList == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _CustomerDto = _mapper.Map<CustomerDTO>(_CustomerList);

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _CustomerDto;

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

        public async Task<ServiceResponse<string>> SoftDeleteCustomerAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingCustomer = await _CustomerRepo.GetCustomerByGUIDAsync(id);

                if (_existingCustomer == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _CustomerRepo.SoftDeleteCustomerAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete Customer!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted Customer";
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

        public async Task<ServiceResponse<CustomerDTO>> UpdateCustomerAsync(Guid id, UpdateCustomerDTO updateCustomerDTO)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var _existingCustomer = await _CustomerRepo.GetCustomerByGUIDAsync(id);

                if (_existingCustomer == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingCustomer = _mapper.Map<UpdateCustomerDTO, Customer>(updateCustomerDTO, _existingCustomer);

                if (!await _CustomerRepo.UpdateCustomerAsync(_existingCustomer))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update Customer!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated Customer";
                _response.Data = _mapper.Map<CustomerDTO>(_existingCustomer);
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

        public async Task<ServiceResponse<TokenCustomer>> CheckCustomerLoginsAsync(string Email, string Password)
        {
            ServiceResponse<TokenCustomer> _response = new();
            try
            {
                var _Customer = await _CustomerRepo.CheckCustomerLoginsAsync(Email, Password);
                if (_Customer == null)
                {
                    _response.Status = false;
                    _response.Title = "Not Found!";
                    return _response;
                }
                else
                {
                    var tokenCustomer = new TokenCustomer()
                    {
                        customerDTO = _mapper.Map<CustomerDTO>(_Customer),
                        token = GenerateToken(_Customer)
                    };
                    _response.Status = true;
                    _response.Title = "OK";
                    _response.Data = tokenCustomer;
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

        private string GenerateToken(Customer customer)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSetting.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
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
