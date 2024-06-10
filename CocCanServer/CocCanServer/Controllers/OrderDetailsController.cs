using CocCanService.DTOs.OrderDetail;
using CocCanService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CocCanService.DTOs.Order;
using Microsoft.AspNetCore.Authorization;

namespace CocCanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailDTO>))]
        public async Task<IActionResult> GetAll(string filter, string range, string sort)
        {
            var orderDetail = await _orderDetailService.GetAllOrderDetailsAsync(filter,range,sort);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            HttpContext.Response.Headers.Add("Content-Range", "orderDetails 0-1/2");
            return Ok(orderDetail.Data);
        }

        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDetailDTO>))]
        public async Task<IActionResult> GetOrderDetailByOrderIdAll(Guid id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
            return Ok(orderDetail.Data);
        }

        [HttpPut("{id:Guid}", Name = "UpdateOrderDetail")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrderDetail(Guid id, [FromBody] UpdateOrderDetailDTO updateOrderDetailDTO)
        {
            var _updateOrderDetail = await _orderDetailService.UpdateOrderDetailAsync(id, updateOrderDetailDTO);

            if (_updateOrderDetail.Status == false && _updateOrderDetail.Title == "RepoError")
            {
                foreach (string error in _updateOrderDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_updateOrderDetail.Status == false && _updateOrderDetail.Title == "Error")
            {
                foreach (string error in _updateOrderDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }
            return Ok(_updateOrderDetail.Data);
        }

        [HttpDelete("{id:Guid}", Name = "DeleteOrderDetail")]
        [Authorize(Roles = "Staff")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //Not found
        [ProducesResponseType(StatusCodes.Status409Conflict)] //Can not be removed 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {

            var _deleteOrderDetail = await _orderDetailService.SoftDeleteOrderDetailAsync(id);

            if (_deleteOrderDetail.Status == false && _deleteOrderDetail.Title == "RepoError")
            {
                foreach (string error in _deleteOrderDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            if (_deleteOrderDetail.Status == false && _deleteOrderDetail.Title == "Error")
            {
                foreach (string error in _deleteOrderDetail.ErrorMessages)
                {
                    ModelState.AddModelError("", error);
                }
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
