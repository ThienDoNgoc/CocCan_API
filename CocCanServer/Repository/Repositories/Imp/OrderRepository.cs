using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.repositories.imp
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CocCanDBContext _dataContext;

        public OrderRepository(CocCanDBContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task<bool> CreateOrderAsync(Order order)
        {
            await _dataContext.Orders.AddAsync(order);
            return await Save();
        }

        public async Task<ICollection<Order>> GetAllOrdersAsync(Dictionary<string, List<string>> filter, int from, int to, string orderBy, bool ascending)
        {
            //return await _dataContext.Orders.Where(e => e.Status==1).ToListAsync();
            IQueryable<Order> _orders =
                _dataContext.Orders.Where(s => s.Status == 1);

            //var stores = _stores
            //    .Join(_dataContext.Products, s => s.Id, p => p.StoreId, (s,p) => new { s = s, p = p });

            if (from <= to & from > 0)
                _orders = _orders.Skip(from - 1).Take(to - from + 1);

            if (filter != null)
                foreach (KeyValuePair<string, List<string>> filterIte in filter)
                {
                    switch (filterIte.Key)
                    {
                        case "customerid":
                            _orders = _orders
                                .Where(m => filterIte.Value.Any(fi => m.CustomerId.ToString() == fi))
                                .Distinct();
                            break;                      
                    }
                }

            if (from <= to & from > 0)
                _orders = _orders.Skip(from - 1).Take(to - from + 1);


            switch (orderBy)
            {
                case "orderTime":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.OrderTime);
                    else
                        _orders = _orders.OrderByDescending(s => s.OrderTime);
                    break;
                case "totalPrice":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.TotalPrice);
                    else
                        _orders = _orders.OrderByDescending(s => s.TotalPrice);
                    break;
                case "customerId":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.CustomerId);
                    else
                        _orders = _orders.OrderByDescending(s => s.CustomerId);
                    break;
                case "sessionId":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.SessionId);
                    else
                        _orders = _orders.OrderByDescending(s => s.SessionId);
                    break;
                case "pickUpSpotId":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.PickUpSpotId);
                    else
                        _orders = _orders.OrderByDescending(s => s.PickUpSpotId);
                    break;
                case "status":
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.OrderStatus);
                    else
                        _orders = _orders.OrderByDescending(s => s.OrderStatus);
                    break;
                case "default":
                    _orders = _orders.OrderBy(s => s.Id);
                    break;
                default:
                    if (ascending)
                        _orders = _orders.OrderBy(s => s.OrderStatus);
                    else
                        _orders = _orders.OrderByDescending(s => s.OrderStatus);
                    break;
            }
            return await _orders.
                                Include(s => s.Session)
                                    .ThenInclude(p => p.Location)
                                .Include(m => m.PickUpSpot)
                                .Include(s => s.Session)
                                    .ThenInclude(p => p.TimeSlot)
                .ToListAsync();
        }

        public async Task<bool> HardDeleteOrderAsync(Order order)
        {
            _dataContext.Remove(order);
            return await Save();
        }

        public async Task<bool> SoftDeleteOrderAsync(Guid id)
        {
            var _existingOrder = await GetOrderByGUIDAsync(id);

            if (_existingOrder != null)
            {
                _existingOrder.Status = 0;
                return await Save();
            }
            return false;
        }

        public Task<bool> UpdateOrderAsync(Order order)
        {
            _dataContext.Orders.Update(order);
            return Save();
        }

        public async Task<Order> GetOrderByGUIDAsync(Guid id)
        {
            return await _dataContext.Orders.
                                Include(s => s.Session)
                                    .ThenInclude(p => p.Location)
                                .Include(m => m.PickUpSpot)
                                .Include(s => s.Session)
                                    .ThenInclude(p => p.TimeSlot)
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        private async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<ICollection<Order>> GetAllOrdersByCustomerAsync(Guid id)
        {
            return await _dataContext.Orders.Where(s => s.CustomerId == id).ToListAsync();
        }
    }
}
