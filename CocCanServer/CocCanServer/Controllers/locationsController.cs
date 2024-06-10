using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanService.DTOs.Location;
using CocCanService.DTOs.OrderDetail;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LocationDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var locations = await _locationService.GetAllLocationsAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "locations 0-1/2");
            return Ok(locations.Data);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LocationDTO>))]
        public async Task<IActionResult> GetLocationByIdAll(Guid id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            return Ok(location.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocationDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LocationDTO>> CreateLocation([FromBody] CreateLocationDTO createLocationDTO)
        {
            if (createLocationDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newLocation = await _locationService.CreateLocationAsync(createLocationDTO);

            if (_newLocation.Status == false && _newLocation.Title == "RepoError")
            {
                foreach (string error in _newLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newLocation.Status == false && _newLocation.Title == "Error")
            {
                foreach (string error in _newLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_newLocation.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateLocation")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] LocationDTO locationDTO)
        {
            if (locationDTO == null || locationDTO.Id != id)
            {
                return BadRequest(ModelState);
            }


            var _updateLocation = await _locationService.UpdateLocationAsync(locationDTO);

            if (_updateLocation.Status == false && _updateLocation.Title == "RepoError")
            {
                foreach (string error in _updateLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateLocation.Status == false && _updateLocation.Title == "Error")
            {
                foreach (string error in _updateLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateLocation.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteLocation")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {

            var _deleteLocation = await _locationService.SoftDeleteLocationAsync(id);

            if (_deleteLocation.Status == false && _deleteLocation.Title == "RepoError")
            {
                foreach (string error in _deleteLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteLocation.Status == false && _deleteLocation.Title == "Error")
            {
                foreach (string error in _deleteLocation.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
