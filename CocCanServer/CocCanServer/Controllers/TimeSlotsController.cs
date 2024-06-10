using CocCanAPI.Filter;
using CocCanService.DTOs.TimeSlot;
using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSlotsController : ControllerBase
    {
        private readonly ITimeSlotService _TimeSlotService;

        public TimeSlotsController(ITimeSlotService TimeSlotService)
        {
            _TimeSlotService = TimeSlotService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TimeSlotDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var _TimeSlots = await _TimeSlotService.GetAllTimeSlotsAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "TimeSlots 0-1/2");

            if (_TimeSlots.Status == false && _TimeSlots.Title == "Error")
            {
                foreach (string error in _TimeSlots.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_TimeSlots.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeSlotDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TimeSlotDTO>> CreateTimeSlot([FromBody] CreateTimeSlotDTO createTimeSlotDTO)
        {
            if (createTimeSlotDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); }

            var _newTimeSlot = await _TimeSlotService.CreateTimeSlotAsync(createTimeSlotDTO);

            if (_newTimeSlot.Status == false && _newTimeSlot.Title == "RepoError")
            {
                foreach (string error in _newTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newTimeSlot.Status == false && _newTimeSlot.Title == "Error")
            {
                foreach (string error in _newTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newTimeSlot.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateTimeSlot")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTimeSlot(Guid id, [FromBody] TimeSlotDTO updateTimeSlotDTO)
        {
            var _updateTimeSlot = await _TimeSlotService.UpdateTimeSlotAsync(updateTimeSlotDTO);

            if (_updateTimeSlot.Status == false && _updateTimeSlot.Title == "NotFound")
            {
                foreach (string error in _updateTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("id", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateTimeSlot.Status == false && _updateTimeSlot.Title == "RepoError")
            {
                foreach (string error in _updateTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("TimeSlotRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateTimeSlot.Status == false && _updateTimeSlot.Title == "Error")
            {
                foreach (string error in _updateTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("Exception", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateTimeSlot.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteTimeSlot")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTimeSlot(Guid id)
        {

            var _deleteTimeSlot = await _TimeSlotService.SoftDeleteTimeSlotAsync(id);

            if (_deleteTimeSlot.Status == false && _deleteTimeSlot.Title == "NotFound")
            {
                foreach (string error in _deleteTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("TimeSlotRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            if (_deleteTimeSlot.Status == false && _deleteTimeSlot.Title == "RepoError")
            {
                foreach (string error in _deleteTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("TimeSlotRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteTimeSlot.Status == false && _deleteTimeSlot.Title == "Error")
            {
                foreach (string error in _deleteTimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("TimeSlotRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TimeSlotDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TimeSlotDTO>> GetByGUID(Guid id)
        {
            var _TimeSlot = await _TimeSlotService.GetTimeSlotByGUIDAsync(id);

            if (_TimeSlot.Status == false && _TimeSlot.Title == "NotFound")
            {
                foreach (string error in _TimeSlot.ErrorMessages)
                {
                    ModelState.AddModelError("TimeSlotRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            return Ok(_TimeSlot.Data);
        }
    }
}
