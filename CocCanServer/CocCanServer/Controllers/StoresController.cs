using CocCanService.DTOs.Store;
using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanAPI.Filter;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;   
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StoreDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var _stores = await _storeService.GetAllStoresWithStatusAsync(filter, range, sort);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "stores 0-1/2");

            if (_stores.Status == false && _stores.Title == "Error")
            {
                foreach (string error in _stores.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_stores.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<StoreDTO>> CreateStore([FromBody] CreateStoreDTO createStoreDTO)
        {
            if (createStoreDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); }

            var _newStore = await _storeService.CreateStoreAsync(createStoreDTO);

            if (_newStore.Status == false && _newStore.Title == "RepoError")
            {
                foreach (string error in _newStore.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newStore.Status == false && _newStore.Title == "Error")
            {
                foreach (string error in _newStore.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newStore.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateStore")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStore(Guid id, [FromBody] UpdateStoreDTO updateStoreDTO)
        {
            var _updateStore = await _storeService.UpdateStoreAsync(id, updateStoreDTO);

            if (_updateStore.Status == false && _updateStore.Title == "NotFound")
            {
                foreach (string error in _updateStore.ErrorMessages)
                {
                    ModelState.AddModelError("id", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateStore.Status == false && _updateStore.Title == "RepoError")
            {
                foreach (string error in _updateStore.ErrorMessages)
                {
                    ModelState.AddModelError("StoreRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateStore.Status == false && _updateStore.Title == "Error")
            {
                foreach (string error in _updateStore.ErrorMessages)
                {
                    ModelState.AddModelError("Exception", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateStore.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteStore")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStore(Guid id)
        {

            var _deleteStore = await _storeService.SoftDeleteStoreAsync(id);

            if (_deleteStore.Status == false && _deleteStore.Title == "NotFound")
            {
                foreach (string error in _deleteStore.ErrorMessages)
                {
                    ModelState.AddModelError("StoreRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            if (_deleteStore.Status == false && _deleteStore.Title == "RepoError")
            {
                foreach (string error in _deleteStore.ErrorMessages)
                {
                    ModelState.AddModelError("StoreRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteStore.Status == false && _deleteStore.Title == "Error")
            {
                foreach (string error in _deleteStore.ErrorMessages)
                {
                    ModelState.AddModelError("StoreRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesDefaultResponseType]
        public async Task<ActionResult<StoreDTO>> GetByGUID(Guid id, string filter)
        {
            var _store = await _storeService.GetStoreByIdAsync(id);

            if (_store.Status == false && _store.Title == "NotFound")
            {
                foreach (string error in _store.ErrorMessages)
                {
                    ModelState.AddModelError("StoreRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            return Ok(_store.Data);
        }
    }
}
