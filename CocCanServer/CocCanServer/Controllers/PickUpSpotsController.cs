using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.PickUpSpot;
using CocCanService.Services;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PickUpSpotsController : ControllerBase
    {
        private readonly IPickUpSpotService _pickUpSpotService;

        public PickUpSpotsController(IPickUpSpotService pickUpSpotService)
        {
            _pickUpSpotService = pickUpSpotService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PickUpSpotDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var pickUpSpot = await _pickUpSpotService.GetAllPickUpSpotsAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "pickUpSpots 0-1/2");
            return Ok(pickUpSpot.Data);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PickUpSpotDTO>))]
        public async Task<IActionResult> GetPickUpSpotByIdAll(Guid id)
        {
            var orderDetail = await _pickUpSpotService.GetPickUpSpotByIdAsync(id);
            return Ok(orderDetail.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PickUpSpotDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PickUpSpotDTO>> CreatePickUpSpot([FromBody] CreatePickUpSpotDTO createPickUpSpotDTO)
        {
            if (createPickUpSpotDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newPickUpSpot = await _pickUpSpotService.CreatePickUpSpotAsync(createPickUpSpotDTO);
            if (_newPickUpSpot.Status == false && _newPickUpSpot.Title == "RepoError")
            {
                foreach (string error in _newPickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newPickUpSpot.Status == false && _newPickUpSpot.Title == "Error")
            {
                foreach (string error in _newPickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newPickUpSpot.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdatePickUpSpot")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePickUpSpot(Guid id, [FromBody] PickUpSpotDTO pickUpSpotDTO)
        {
            if (pickUpSpotDTO == null || pickUpSpotDTO.Id != id)
            {
                return BadRequest(ModelState);
            }


            var _updatePickUpSpot = await _pickUpSpotService.UpdatePickUpSpotAsync(pickUpSpotDTO);
            if (_updatePickUpSpot.Status == false && _updatePickUpSpot.Title == "RepoError")
            {
                foreach (string error in _updatePickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updatePickUpSpot.Status == false && _updatePickUpSpot.Title == "Error")
            {
                foreach (string error in _updatePickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_updatePickUpSpot.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeletePickUpSpot")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePickUpSpot(Guid id)
        {

            var _deletePickUpSpot = await _pickUpSpotService.SoftDeletePickUpSpotAsync(id);


            if (_deletePickUpSpot.Status == false && _deletePickUpSpot.Title == "RepoError")
            {
                foreach (string error in _deletePickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deletePickUpSpot.Status == false && _deletePickUpSpot.Title == "Error")
            {
                foreach (string error in _deletePickUpSpot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
