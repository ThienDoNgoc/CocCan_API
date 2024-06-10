using AutoMapper;
using CocCanAPI.Filter;
using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.Session;
using CocCanService.Services;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _SessionService;

        public SessionsController(ISessionService SessionService)
        {
            _SessionService = SessionService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Session>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var Sessions = await _SessionService.GetAllSessionsAsync(filter);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "sessions 0-1/2");
            return Ok(Sessions.Data);
        }
        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SessionDTO>> CreateSession([FromBody] CreateSessionDTO createSessionDTO)
        {
            if (createSessionDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); }

            var _newSession = await _SessionService.CreateSessionAsync(createSessionDTO);

            if (_newSession.Status == false && _newSession.Title == "RepoError")
            {
                foreach (string error in _newSession.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_newSession.Status == false && _newSession.Title == "Error")
            {
                foreach (string error in _newSession.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_newSession.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateSession")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSession(Guid id, [FromBody] SessionDTO updateSessionDTO)
        {
            var _updateSession = await _SessionService.UpdateSessionAsync(updateSessionDTO);

            if (_updateSession.Status == false && _updateSession.Title == "NotFound")
            {
                foreach (string error in _updateSession.ErrorMessages)
                {
                    ModelState.AddModelError("id", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateSession.Status == false && _updateSession.Title == "RepoError")
            {
                foreach (string error in _updateSession.ErrorMessages)
                {
                    ModelState.AddModelError("SessionRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateSession.Status == false && _updateSession.Title == "Error")
            {
                foreach (string error in _updateSession.ErrorMessages)
                {
                    ModelState.AddModelError("Exception", error);
                }
                return StatusCode(500, ModelState);
            }

            return Ok(_updateSession.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteSession")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSession(Guid id)
        {

            var _deleteSession = await _SessionService.SoftDeleteSessionAsync(id);

            if (_deleteSession.Status == false && _deleteSession.Title == "NotFound")
            {
                foreach (string error in _deleteSession.ErrorMessages)
                {
                    ModelState.AddModelError("SessionRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            if (_deleteSession.Status == false && _deleteSession.Title == "RepoError")
            {
                foreach (string error in _deleteSession.ErrorMessages)
                {
                    ModelState.AddModelError("SessionRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteSession.Status == false && _deleteSession.Title == "Error")
            {
                foreach (string error in _deleteSession.ErrorMessages)
                {
                    ModelState.AddModelError("SessionRepo", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SessionDTO>> GetByGUID(Guid id)
        {
            var _Session = await _SessionService.GetSessionByGUIDAsync(id);

            if (_Session.Status == false && _Session.Title == "NotFound")
            {
                foreach (string error in _Session.ErrorMessages)
                {
                    ModelState.AddModelError("SessionRepo", error);
                }
                return StatusCode(404, ModelState);
            }

            return Ok(_Session.Data);
        }
    }
}
