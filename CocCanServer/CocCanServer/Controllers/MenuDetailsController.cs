using CocCanService.DTOs.Menu;
using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanService.DTOs.MenuDetail;
using CocCanService.DTOs.OrderDetail;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuDetailsController : ControllerBase
    {
        private readonly IMenuDetailService _menuDetailService;

        public MenuDetailsController(IMenuDetailService menuDetailService)
        {
            _menuDetailService = menuDetailService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuDetailDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var menuDetail = await _menuDetailService.GetAllMenuDetailsAsync(filter,range,sort);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "menuDetails 0-1/2");
            if (menuDetail.Status == false && menuDetail.Title == "Error")
            {
                foreach (string error in menuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(menuDetail.Data);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuDetailDTO>))]
        public async Task<IActionResult> GetMenuDetailByIdAll(Guid id)
        {
            var orderDetail = await _menuDetailService.GetMenuDetailByIdAsync(id);
            return Ok(orderDetail.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuDetailDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MenuDetailDTO>> CreateMenuDetail([FromBody] CreateMenuDetailDTO createMenuDetailDTO)
        {
            if (createMenuDetailDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newMenuDetail = await _menuDetailService.CreateMenuDetailAsync(createMenuDetailDTO);

            if (_newMenuDetail.Status == false && _newMenuDetail.Title == "RepoError")
            {
                foreach (string error in _newMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newMenuDetail.Status == false && _newMenuDetail.Title == "Error")
            {
                foreach (string error in _newMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newMenuDetail.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateMenuDetail")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMenuDetail(Guid id, [FromBody] MenuDetailDTO menuDetailDTO)
        {
            if (menuDetailDTO == null || menuDetailDTO.Id != id)
            {
                return BadRequest(ModelState);
            }


            var _updateMenuDetail = await _menuDetailService.UpdateMenuDetailAsync(menuDetailDTO);

            if (_updateMenuDetail.Status == false && _updateMenuDetail.Title == "RepoError")
            {
                foreach (string error in _updateMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateMenuDetail.Status == false && _updateMenuDetail.Title == "Error")
            {
                foreach (string error in _updateMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateMenuDetail.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteMenuDetail")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMenuDetail(Guid id)
        {

            var _deleteMenuDetail = await _menuDetailService.HardDeleteMenuDetailAsync(id);

            if (_deleteMenuDetail.Status == false && _deleteMenuDetail.Title == "RepoError")
            {
                foreach (string error in _deleteMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteMenuDetail.Status == false && _deleteMenuDetail.Title == "Error")
            {
                foreach (string error in _deleteMenuDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
