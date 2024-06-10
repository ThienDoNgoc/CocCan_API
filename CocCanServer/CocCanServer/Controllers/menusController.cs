using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanService.DTOs.Menu;
using CocCanService.DTOs.OrderDetail;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var menu = await _menuService.GetAllMenusAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "menus 0-1/2");
            return Ok(menu.Data);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuDTO>))]
        public async Task<IActionResult> GetMenuByIdAll(Guid id)
        {
            var orderDetail = await _menuService.GetMenuByIdAsync(id);
            return Ok(orderDetail.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<MenuDTO>> CreateMenu([FromBody] CreateMenuDTO createMenuDTO)
        {
            if (createMenuDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newMenu = await _menuService.CreateMenuAsync(createMenuDTO);

            if (_newMenu.Status == false && _newMenu.Title == "RepoError")
            {
                foreach (string error in _newMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newMenu.Status == false && _newMenu.Title == "Error")
            {
                foreach (string error in _newMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_newMenu.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateMenu")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] UpdateMenuDTO updateMenuDTO)
        {
            var _updateMenu = await _menuService.UpdateMenuAsync(id, updateMenuDTO);

            if (_updateMenu.Status == false && _updateMenu.Title == "RepoError")
            {
                foreach (string error in _updateMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateMenu.Status == false && _updateMenu.Title == "Error")
            {
                foreach (string error in _updateMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateMenu.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteMenu")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {

            var _deleteMenu = await _menuService.SoftDeleteMenuAsync(id);

            if (_deleteMenu.Status == false && _deleteMenu.Title == "RepoError")
            {
                foreach (string error in _deleteMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteMenu.Status == false && _deleteMenu.Title == "Error")
            {
                foreach (string error in _deleteMenu.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
