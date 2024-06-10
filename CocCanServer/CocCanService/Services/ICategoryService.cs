using CocCanService.DTOs.Category;
using CocCanService.DTOs.Location;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<DTOs.Category.CategoryDTO>>> GetAllCategoriesAsync();
        Task<ServiceResponse<DTOs.Category.CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO);
        Task<ServiceResponse<DTOs.Category.CategoryDTO>> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateCategoryDTO);
        Task<ServiceResponse<DTOs.Category.CategoryDTO>> GetCategoryByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteCategoryAsync(Guid id);
    }
}
