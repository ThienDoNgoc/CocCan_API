using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetAllOrdersAsync(Dictionary<string, List<string>> filter, int from, int to, string orderBy, bool ascending);
        Task<ICollection<Order>> GetAllOrdersByCustomerAsync(Guid id);
        Task<bool> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> SoftDeleteOrderAsync(Guid id);
        Task<bool> HardDeleteOrderAsync(Order order);
        Task<Order> GetOrderByGUIDAsync(Guid id);
    }
}
