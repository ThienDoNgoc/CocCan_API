using CocCanService.DTOs.OrderDetail;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IOrderDetailService
    {
        Task<ServiceResponse<List<DTOs.OrderDetail.OrderDetailDTO>>> GetAllOrderDetailsAsync(string filter, string range, string sort);
        Task<ServiceResponse<List<DTOs.OrderDetail.OrderDetailDTO>>> GetAllOrderDetailsByOrderIDAsync(Guid orderID);
        Task<ServiceResponse<List<DTOs.OrderDetail.OrderDetailBatchDTO>>> GetOrderDetailByBatch(Guid orderID);
        Task<ServiceResponse<OrderDetailDTO>> CreateOrderDetailAsync(CreateOrderDetailDTO createOrderDetailDTO, Guid id);
        Task<ServiceResponse<DTOs.OrderDetail.OrderDetailDTO>> UpdateOrderDetailAsync(Guid id, UpdateOrderDetailDTO updateOrderDetailDTO);
        Task<ServiceResponse<DTOs.OrderDetail.OrderDetailDTO>> GetOrderDetailByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteOrderDetailAsync(Guid id);
    }
}
