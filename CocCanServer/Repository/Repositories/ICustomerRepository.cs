using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface ICustomerRepository
    {
        Task<ICollection<Customer>>
            GetAllCustomersWithStatusAsync();
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<bool> CreateCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        Task<bool> SoftDeleteCustomerAsync(Guid id);
        Task<Customer> GetCustomerByGUIDAsync(Guid id);
        Task<Customer> CheckCustomerLoginsAsync(string Email, string Password);
    }
}
