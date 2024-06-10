using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.repositories.imp
{
    public class MenuDetailRepository : IMenuDetailRepository
    {
        private readonly CocCanDBContext _dataContext;

        public MenuDetailRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateMenuDetailAsync(MenuDetail menuDetail)
        {
            await _dataContext.MenuDetails.AddAsync(menuDetail);
            return await Save();
        }
        //d411a66c-0315-4d24-b659-100891ce2628
        //7c25b7a9-26ca-4d08-84f6-cda301e862ef
        public async Task<ICollection<MenuDetail>> GetAllMenuDetailsAsync(Dictionary<string, List<string>> filter, int from, int to, string orderBy, bool ascending)
        {
            IQueryable<MenuDetail> menuDetails =
                _dataContext.MenuDetails.Where(s => s.Status == 1);
            switch (orderBy)
            {
                case "menuId":
                    if (ascending)
                        menuDetails = menuDetails.OrderBy(s => s.MenuId);
                    else
                        menuDetails = menuDetails.OrderByDescending(s => s.MenuId);
                    break;
                case "default":
                    menuDetails = menuDetails.OrderBy(s => s.Id);
                    break;
                default:
                    if (ascending)
                        menuDetails = menuDetails.OrderBy(s => s.Id);
                    else
                        menuDetails = menuDetails.OrderByDescending(s => s.Id);
                    break;
            }

            if (filter != null)
                foreach (KeyValuePair<string, List<string>> filterIte in filter)
                {
                    switch (filterIte.Key)
                    {
                        case "session":
                            menuDetails = menuDetails
                                .Join(_dataContext.Menus
                                    .Join(_dataContext.Sessions.Where(s => filter["session"][0] == s.Id.ToString()),
                                    m => m.Id,
                                    s => s.MenuId,
                                    (m, s) => m),
                                md => md.MenuId,
                                m => m.Id,
                                (md, m) => md);
                            break;
                        case "store":
                            menuDetails = menuDetails
                                .Join(_dataContext.Products
                                    .Join(_dataContext.Stores.Where(s => filter["store"][0] == s.Id.ToString()),
                                    p => p.StoreId,
                                    s => s.Id,
                                    (p, s) => p),
                                md => md.ProductId,
                                m => m.Id,
                                (md, m) => md);
                            break;
                        case "id":
                            menuDetails = menuDetails
                                .Where(md => filterIte.Value.Any(fi => fi == md.Id.ToString()));
                            break;
                    }
                }

            return await menuDetails
                    .Distinct()
                    .Include(md => md.Product)
                        .ThenInclude(p => p.Category)
                    .ToListAsync();
        }

        public async Task<bool> HardDeleteMenuDetailAsync(Guid id)
        {
            var _existingMenuDetail = await GetMenuDetailByGUIDAsync(id);
            _dataContext.Remove(_existingMenuDetail);
            return await Save();
        }

        //public async Task<bool> SoftDeleteMenuDetailAsync(Guid id)
        //{
        //    var _existingMenu = await GetMenuByGUIDAsync(id);

        //    if (_existingMenu != null)
        //    {
        //        _existingMenu.Status = 0;
        //        return await Save();
        //    }
        //    return false;
        //}

        public Task<bool> UpdateMenuDetailAsync(MenuDetail menuDetail)
        {
            _dataContext.MenuDetails.Update(menuDetail);
            return Save();
        }

        public async Task<MenuDetail> GetMenuDetailByGUIDAsync(Guid id)
        {
            return await _dataContext.MenuDetails
                .Include(md => md.Product)
                    .ThenInclude(p => p.Category)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
