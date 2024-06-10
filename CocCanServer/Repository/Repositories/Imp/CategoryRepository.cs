using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace Repository.repositories.imp
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly CocCanDBContext _dataContext;

        public CategoryRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _dataContext.Categories.AddAsync(category);
            return await Save();
        }

        public async Task<ICollection<Category>> 
            GetAllCategorysWithStatusAsync()
        {
            IQueryable<Category> _categories =
                _dataContext.Categories.Where(c => c.Status == 1);

            return await _categories
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByGUIDAsync(Guid id)
        {
            return await _dataContext.Categories
                .Where(s => s.Status == 1)
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SoftDeleteCategoryAsync(Guid id)
        {
            var _existingCategory = await GetCategoryByGUIDAsync(id);

            if (_existingCategory != null)
            {
                _existingCategory.Status = 0;
                return await Save();
            }
            return false;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _dataContext.Categories.Update(category);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }
    }
}
