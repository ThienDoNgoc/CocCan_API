using AutoMapper;
using CocCanService.DTOs.Category;
using CocCanService.DTOs.Store;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
        {
            this._categoryRepo = categoryRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var _newCategory = _mapper.Map<Category>(createCategoryDTO);

                if (!await _categoryRepo.CreateCategoryAsync(_newCategory))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Category Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }
                
                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<CategoryDTO>(_newCategory);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<List<CategoryDTO>>> GetAllCategoriesAsync()
        {
            ServiceResponse<List<CategoryDTO>> _response = new();
            try
            {
                var _CategoryList = await _categoryRepo.GetAllCategorysWithStatusAsync();

                var _CategoryListDTO = new List<CategoryDTO>();

                foreach (var item in _CategoryList)
                {
                    _CategoryListDTO.Add(_mapper.Map<CategoryDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all categories";
                _response.Data = _CategoryListDTO;
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<CategoryDTO>> GetCategoryByIdAsync(Guid id)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var _CategoryList = await _categoryRepo.GetCategoryByGUIDAsync(id);

                if (_CategoryList == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _CategoryDto = _mapper.Map<CategoryDTO>(_CategoryList);

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _CategoryDto;

            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<string>> SoftDeleteCategoryAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingCategory = await _categoryRepo.GetCategoryByGUIDAsync(id);

                if (_existingCategory == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _categoryRepo.SoftDeleteCategoryAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted category";
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<CategoryDTO>> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateCategoryDTO)
        {
            ServiceResponse<CategoryDTO> _response = new();
            try
            {
                var _existingCategory = await _categoryRepo.GetCategoryByGUIDAsync(id);

                if (_existingCategory == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingCategory = _mapper.Map<UpdateCategoryDTO, Category>(updateCategoryDTO, _existingCategory);

                if (!await _categoryRepo.UpdateCategoryAsync(_existingCategory))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated category";
                _response.Data = _mapper.Map<CategoryDTO>(_existingCategory);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }
    }
}
