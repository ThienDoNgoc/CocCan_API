using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.repositories.imp
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CocCanDBContext _dataContext;

        public CustomerRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            await _dataContext.Customers.AddAsync(customer);
            return await Save();
        }

        public async Task<ICollection<Customer>>
            GetAllCustomersWithStatusAsync()
        {
            IQueryable<Customer> _customers =
                _dataContext.Customers.Where(c => c.Status == 1);

            return await _customers
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByGUIDAsync(Guid id)
        {
            return await _dataContext.Customers
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _dataContext.Customers
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Email == email);
        }

        public async Task<bool> SoftDeleteCustomerAsync(Guid id)
        {
            var _existingCustomer = await GetCustomerByGUIDAsync(id);

            if (_existingCustomer != null)
            {
                _existingCustomer.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateCustomerAsync(Customer Customer)
        {
            _dataContext.Customers.Update(Customer);
            return await Save();
        }

        public async Task<Customer> CheckCustomerLoginsAsync(string UserName, string Password)
        {
            return await _dataContext.Customers.SingleOrDefaultAsync(sta => sta.Username == UserName && sta.Password == Password);
        }


        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
