using CocCanService.DTOs.Customer;
using CocCanService.DTOs.Staff;
using CocCanService.Services.Imp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<DTOs.Customer.CustomerDTO>>> GetAllCustomersAsync();
        Task<ServiceResponse<TokenCustomer>> CheckCustomerLoginsAsync(string Email, string Password);
        Task<ServiceResponse<DTOs.Customer.CustomerDTO>> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO);
        Task<ServiceResponse<DTOs.Customer.CustomerDTO>> UpdateCustomerAsync(Guid id, UpdateCustomerDTO updateCustomerDTO);
        Task<ServiceResponse<DTOs.Customer.CustomerDTO>> GetCustomerByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteCustomerAsync(Guid id);
    }
}
