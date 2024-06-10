using CocCanService.DTOs.PickUpSpot;
using CocCanService.DTOs.Customer;
using CocCanService.Services;
using CocCanService.Services.Imp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _CustomerService;

        public CustomersController(ICustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var Customers = await _CustomerService.GetAllCustomersAsync();
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "Customers 0-1/2");
            return Ok(Customers.Data);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerDTO>))]
        public async Task<IActionResult> GetCustomerByIdAll(Guid id)
        {
            var orderDetail = await _CustomerService.GetCustomerByIdAsync(id);
            return Ok(orderDetail.Data);
        }

        [HttpPost("Authen")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CustomerDTO>> CheckCustomerLogin([FromBody] LoginCustomerDTO loginCustomer)
        {   
            if (loginCustomer.UserName == "" || loginCustomer.Password == "" || loginCustomer.UserName == null || loginCustomer.Password == null)
            {
                return BadRequest();
            }
            var CustomerFound = await _CustomerService.CheckCustomerLoginsAsync(loginCustomer.UserName, loginCustomer.Password);

            if (CustomerFound.Data == null)
            {
                return NotFound();
            }
            return Ok(CustomerFound.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer([FromBody] CreateCustomerDTO createCustomerDTO)
        {
            if (createCustomerDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var _newCustomer = await _CustomerService.CreateCustomerAsync(createCustomerDTO);

            if (_newCustomer.Status == false && _newCustomer.Title == "Exist")
            {
                return Ok(_newCustomer);
            }


            if (_newCustomer.Status == false && _newCustomer.Title == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Customer {createCustomerDTO}");
                return StatusCode(500, ModelState);
            }

            if (_newCustomer.Status == false && _newCustomer.Title == "Error")
            {   
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Customer {createCustomerDTO}");
                return StatusCode(500, ModelState);
            }

            //Return new Customer created
            return Ok(_newCustomer.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateCustomer")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerDTO CustomerDTO)
        {
            if (CustomerDTO == null || CustomerDTO.Id != id)
            {
                return BadRequest(ModelState);
            }


            var _updateCustomer = await _CustomerService.UpdateCustomerAsync(id, CustomerDTO);

            if (_updateCustomer.Status == false && _updateCustomer.Title == "NotFound")
            {
                return Ok(_updateCustomer);
            }

            if (_updateCustomer.Status == false && _updateCustomer.Title == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Customer {CustomerDTO}");
                return StatusCode(500, ModelState);
            }

            if (_updateCustomer.Status == false && _updateCustomer.Title == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Customer {CustomerDTO}");
                return StatusCode(500, ModelState);
            }

            return Ok(_updateCustomer.Data);
        }
    }
}
