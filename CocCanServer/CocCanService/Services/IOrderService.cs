using CocCanService.DTOs.Category;
using CocCanService.DTOs.Order;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<DTOs.Order.OrderDTO>>> GetAllOrdersAsync(string filter, string range, string sort);  
        Task<ServiceResponse<List<DTOs.Order.OrderDTO>>> GetAllOrdersByCustomerIdAsync(Guid customerId);
        Task<ServiceResponse<DTOs.Order.OrderDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        Task<ServiceResponse<DTOs.Order.OrderDTO>> UpdateOrderAsync(OrderDTO orderDTO);
        Task<ServiceResponse<DTOs.Order.OrderDTO>> GetOrderByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteOrderAsync(Guid id);
    }
}
