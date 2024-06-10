using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.repositories.imp
{
    public class StaffRepository : IStaffRepository
    {
        private readonly CocCanDBContext _dataContext;

        public StaffRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<Staff> CheckStaffLoginsAsync(string UserName, string Password)
        {
            return await _dataContext.Staffs.SingleOrDefaultAsync(sta => sta.Username == UserName && sta.Password == Password);
        }

        public async Task<bool> CreateStaffAsync(Staff staff)
        {
            await _dataContext.Staffs.AddAsync(staff);
            return await Save();
        }

        public async Task<ICollection<Staff>> GetAllStaffsAsync()
        {
            return await _dataContext.Staffs.ToListAsync();
        }

        public async Task<bool> SoftDeleteStaffAsync(Guid id)
        {
            var _existingStaff = await GetStaffAsync(id);
            
            if (_existingStaff != null)
            {
                _existingStaff.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateStaffAsync(Staff staff)
        {
            _dataContext.Staffs.Update(staff);
            return await Save();
        }

        public async Task<Staff> GetStaffAsync(Guid id)
        {
            return await _dataContext.Staffs.SingleOrDefaultAsync(s => s.Id == id);
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
