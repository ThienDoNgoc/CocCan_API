using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanService.DTOs.Category;
using Microsoft.AspNetCore.Authorization;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "categories 0-1/2");
            if (categories.Status == false && categories.Title == "Error")
            {
                foreach (string error in categories.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(categories.Data);
        }
        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDTO>))]
        public async Task<IActionResult> GetCategoryByIdAll(Guid id)
        {
            var categories = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(categories.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            if (createCategoryDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newCategory = await _categoryService.CreateCategoryAsync(createCategoryDTO);

            if (_newCategory.Status == false && _newCategory.Title == "Error")
            {
                foreach (string error in _newCategory.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newCategory.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateCategory")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            if (updateCategoryDTO == null)
            {
                return BadRequest(ModelState);
            }

            var _updateCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDTO);

            if (_updateCategory.Status == false && _updateCategory.Title == "Error")
            {
                foreach (string error in _updateCategory.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateCategory.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteCategory")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {

            var _deleteCategory = await _categoryService.SoftDeleteCategoryAsync(id);

            if (_deleteCategory.Status == false && _deleteCategory.Title == "Error")
            {   
                foreach (string error in _deleteCategory.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
