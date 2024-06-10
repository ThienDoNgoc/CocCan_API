using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface IOrderDetailRepository
    {
        Task<ICollection<OrderDetail>> GetAllOrderDetailsAsync(Dictionary<string, List<string>> filter, int from, int to, string orderBy, bool ascending);
        Task<ICollection<OrderDetail>> GetAllOrderDetailsByOrderIDAsync(Guid orderId);
        Task<ICollection<OrderDetail>> GetOrderDetailByBatch(Guid sessionId, Guid storeId);
        Task<bool> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> SoftDeleteOrderDetailAsync(Guid id);
        Task<bool> HardDeleteOrderDetailAsync(OrderDetail orderDetail);
        public int CountAllItemAsync(Guid id);
        Task<OrderDetail> GetOrderDetailByGUIDAsync(Guid id);
    }
}
