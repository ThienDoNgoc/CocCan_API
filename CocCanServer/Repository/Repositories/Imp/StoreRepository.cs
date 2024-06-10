using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.repositories.imp
{
    public class StoreRepository : IStoreRepository
    {
        private readonly CocCanDBContext _dataContext;

        public StoreRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateStoreAsync(Store store)
        {
            await _dataContext.Stores.AddAsync(store);
            return await Save();
        }

        public async Task<ICollection<Store>> 
            GetAllStoresWithStatusAsync
            (Dictionary<string, List<string>> filter, int from, int to, string orderBy, bool ascending)
        {
            IQueryable<Store> _stores =
                _dataContext.Stores.Where(s => s.Status == 1);

            //var stores = _stores
            //    .Join(_dataContext.Products, s => s.Id, p => p.StoreId, (s,p) => new { s = s, p = p });

            switch (orderBy)
            {
                case "name":
                    if (ascending)
                        _stores = _stores.OrderBy(s => s.Name);
                    else
                        _stores = _stores.OrderByDescending(s => s.Name);
                    break;
                case "default":
                    _stores = _stores.OrderBy(s => s.Id);
                    break;
                default:
                    if (ascending)
                        _stores = _stores.OrderBy(s => s.Id);
                    else
                        _stores = _stores.OrderByDescending(s => s.Id);
                    break;
            }

            if (filter != null)
                foreach (KeyValuePair<string, List<string>> filterIte in filter)
                {
                    switch (filterIte.Key)
                    {
                        case "session":
                            _stores = _stores
                                .Join(_dataContext.Products
                                    .Join(_dataContext.MenuDetails
                                        .Join(_dataContext.Menus
                                            .Join(_dataContext.Sessions.Where(s => filterIte.Value.Any(fi => s.Id.ToString() == fi)),
                                            m => m.Id,
                                            s => s.MenuId,
                                            (s, m) => s),
                                        md => md.MenuId,
                                        m => m.Id,
                                        (m, md) => m),
                                    p => p.Id,
                                    md => md.ProductId,
                                    (p, md) => p),
                                s => s.Id,
                                p => p.StoreId,
                                (s, p) => s)
                                .Distinct();
                            break;
                        case "menu":
                            _stores = _stores
                                .Join(_dataContext.Products
                                    .Join(_dataContext.MenuDetails
                                        .Join(_dataContext.Menus.Where(m => filterIte.Value.Any(fi => m.Id.ToString() == fi)),
                                        md => md.MenuId,
                                        m => m.Id,
                                        (m, md) => m),
                                    p => p.Id,
                                    md => md.ProductId,
                                    (p, md) => p),
                                s => s.Id,
                                p => p.StoreId,
                                (s, p) => s)
                                .Distinct();
                            break;
                        case "name":
                            _stores = _stores.Where(s => filterIte.Value.Any(fi => s.Name == fi));
                            break;
                    }
                }

            if (from <= to & from > 0)
                _stores = _stores.Skip(from - 1).Take(to - from + 1);
            return await _stores
                .ToListAsync();
        }

        public async Task<Store> GetStoreByGUIDAsync(Guid id)
        {
            return await _dataContext.Stores
                .Where(s => s.Status == 1)
                .Include(s => s.Products)
                    .ThenInclude(p => p.Category)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteStoreAsync(Guid id)
        {
            var _existingStore = await GetStoreByGUIDAsync(id);

            if (_existingStore != null)
            {
                _existingStore.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            _dataContext.Stores.Update(store);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }

        private bool CheckSearch(string source, List<string> filterStrings)
        {
            foreach (string filterString in filterStrings)
                if (source.Contains(filterString)) return true;
            return false;
        }
    }
}
