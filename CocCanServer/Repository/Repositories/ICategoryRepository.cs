using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>>
            GetAllCategorysWithStatusAsync();
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> SoftDeleteCategoryAsync(Guid id);
        Task<Category> GetCategoryByGUIDAsync(Guid id);
    }
}
