using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace Repository.repositories.imp
{
    public class MenuRepository : IMenuRepository
    {
        private readonly CocCanDBContext _dataContext;

        public MenuRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<bool> CreateMenuAsync(Menu menu)
        {
            await _dataContext.Menus.AddAsync(menu);
            return await Save();
        }

        public async Task<ICollection<Menu>> 
            GetAllMenusWithStatusAsync()
        {
            IQueryable<Menu> _menus =
                _dataContext.Menus.Where(s => s.Status == 1);

            return await _menus
                .ToListAsync();
        }

        public async Task<Menu> GetMenuByGUIDAsync(Guid id)
        {
            return await _dataContext.Menus
                .Where(s => s.Status == 1)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteMenuAsync(Guid id)
        {
            var _existingMenu = await GetMenuByGUIDAsync(id);

            if (_existingMenu != null)
            {
                _existingMenu.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateMenuAsync(Menu menu)
        {
            _dataContext.Menus.Update(menu);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
