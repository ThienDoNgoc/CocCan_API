using AutoMapper;
using CocCanService.DTOs.Order;
using CocCanService.DTOs.Store;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;      
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IMapper _mapper;
        
        public OrderService(IOrderRepository orderRepo,IOrderDetailRepository orderDetail, IMapper mapper)
        {
            this._orderRepo = orderRepo;
            this._orderDetailRepo = orderDetail;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<OrderDTO>> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {            
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                Repository.Entities.Order _newOrder = new()
                {
                    Id = Guid.NewGuid(),
                    OrderTime = createOrderDTO.OrderTime,
                    ServiceFee = createOrderDTO.ServiceFee,
                    TotalPrice = createOrderDTO.TotalPrice,
                    CustomerId = createOrderDTO.CustomerId,
                    SessionId = createOrderDTO.SessionId,
                    PickUpSpotId = createOrderDTO.PickUpSpotId,
                    Note = createOrderDTO.Note,
                    Phone = createOrderDTO.Phone,
                    DeliveryFee = createOrderDTO.DeliveryFee,
                    CartTotalAmount = createOrderDTO.CartTotalAmount,
                    OrderStatus = 0,
                    Status = 1
                };

                if (!await _orderRepo.CreateOrderAsync(_newOrder))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Category Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<OrderDTO>(_newOrder);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }     
            return _response;
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetAllOrdersAsync(string filter, string range, string sort)
        {
            ServiceResponse<List<OrderDTO>> _response = new();
            
            try
            {
                Dictionary<string, List<string>> _filter = null;
                List<int> _range;
                List<string> _sort;
                try
                {
                    if (filter != null)
                        _filter = System.Text.Json.JsonSerializer
                            .Deserialize<Dictionary<string, List<string>>>(filter);
                }
                catch
                {
                    var raw = System.Text.Json.JsonSerializer
                        .Deserialize<Dictionary<string, string>>(filter);
                    _filter = new Dictionary<string, List<string>>();
                    foreach (var item in raw)
                        _filter.Add(item.Key, new List<string>() { item.Value });
                }
                if (range != null)
                    _range = System.Text.Json.JsonSerializer.Deserialize<List<int>>(range);
                else
                    _range = new List<int>() { -1, -1 };
                if (sort != null)
                    _sort = System.Text.Json.JsonSerializer.Deserialize<List<string>>(sort);
                else
                    _sort = new List<string>() { "default", "" };

                var _OrderList = await _orderRepo
                    .GetAllOrdersAsync(
                        _filter, _range[0] + 1, _range[1] + 1, _sort[0], (_sort[1] == "ASC")
                    );
                var _OrderListDTO = new List<OrderDTO>();

                foreach (var item in _OrderList)
                {

                   
                    OrderDTO orderDTO = new OrderDTO();
                    orderDTO.Id = item.Id;
                    orderDTO.OrderTime = item.OrderTime;
                    orderDTO.ServiceFee = item.ServiceFee;
                    orderDTO.Note = item.Note;
                    orderDTO.Phone = item.Phone;
                    orderDTO.DeliveryFee = item.DeliveryFee;
                    orderDTO.CartTotalAmount = item.CartTotalAmount;
                    orderDTO.TotalPrice = item.TotalPrice;
                    orderDTO.CustomerId = item.CustomerId;
                    orderDTO.SessionId = item.SessionId;
                    orderDTO.PickUpSpotId = item.PickUpSpotId;
                    orderDTO.PickUpSpotFullName = item.PickUpSpot.Fullname;
                    orderDTO.LocationID = item.Session.LocationId;
                    orderDTO.LocationName = item.Session.Location.Name;
                    orderDTO.TimeSlotID = item.Session.TimeSlotId;
                    orderDTO.TimeSlotStart = item.Session.TimeSlot.StartTime.ToString();
                    orderDTO.TimeSlotEnd = item.Session.TimeSlot.EndTime.ToString();
                    orderDTO.OrderDetailCount = _orderDetailRepo.CountAllItemAsync(item.Id);
                    orderDTO.OrderStatus = item.OrderStatus;
                    _OrderListDTO.Add(orderDTO);
                }

                _response.Status = true;
                _response.Title = "Got all Orders";
                _response.Data = _OrderListDTO;
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetAllOrdersByCustomerIdAsync(Guid customerId)
        {
            ServiceResponse<List<OrderDTO>> _response = new();
            try
            {
                var _OrderList = await _orderRepo.GetAllOrdersByCustomerAsync(customerId);

                var _OderListDTO = new List<OrderDTO>();

                foreach (var item in _OrderList)
                {
                    _OderListDTO.Add(_mapper.Map<OrderDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all categories";
                _response.Data = _OderListDTO;
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<OrderDTO>> GetOrderByIdAsync(Guid id)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var item = await _orderRepo.GetOrderByGUIDAsync(id);

                if (item == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                OrderDTO orderDTO = new OrderDTO();
                orderDTO.Id = item.Id;
                orderDTO.OrderTime = item.OrderTime;
                orderDTO.ServiceFee = item.ServiceFee;
                orderDTO.Note = item.Note;
                orderDTO.Phone = item.Phone;
                orderDTO.DeliveryFee = item.DeliveryFee;
                orderDTO.CartTotalAmount = item.CartTotalAmount;
                orderDTO.TotalPrice = item.TotalPrice;
                orderDTO.CustomerId = item.CustomerId;
                orderDTO.SessionId = item.SessionId;
                orderDTO.PickUpSpotId = item.PickUpSpotId;
                orderDTO.PickUpSpotFullName = item.PickUpSpot.Fullname;
                orderDTO.LocationID = item.Session.LocationId;
                orderDTO.LocationName = item.Session.Location.Name;
                orderDTO.TimeSlotID = item.Session.TimeSlotId;
                orderDTO.TimeSlotStart = item.Session.TimeSlot.StartTime.ToString();
                orderDTO.TimeSlotEnd = item.Session.TimeSlot.EndTime.ToString();
                orderDTO.OrderDetailCount = _orderDetailRepo.CountAllItemAsync(item.Id);
                orderDTO.OrderStatus = item.OrderStatus;

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = orderDTO;

            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<string>> SoftDeleteOrderAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingOrder = await _orderRepo.GetOrderByGUIDAsync(id);

                if (_existingOrder == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _orderRepo.SoftDeleteOrderAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted category";
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }

        public async Task<ServiceResponse<OrderDTO>> UpdateOrderAsync(OrderDTO orderDTO)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var _existingOrder = await _orderRepo.GetOrderByGUIDAsync(orderDTO.Id);

                if (_existingOrder == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }
                _existingOrder.OrderTime = orderDTO.OrderTime;
                _existingOrder.ServiceFee = orderDTO.ServiceFee;
                _existingOrder.DeliveryFee = orderDTO.DeliveryFee;
                _existingOrder.CartTotalAmount = orderDTO.CartTotalAmount;
                _existingOrder.Note = orderDTO.Note;
                _existingOrder.Phone = orderDTO.Phone;
                _existingOrder.TotalPrice = orderDTO.TotalPrice;
                _existingOrder.CustomerId = orderDTO.CustomerId;
                _existingOrder.SessionId = orderDTO.SessionId;
                _existingOrder.PickUpSpotId = orderDTO.PickUpSpotId;
                _existingOrder.OrderStatus = orderDTO.OrderStatus;

                if (!await _orderRepo.UpdateOrderAsync(_existingOrder))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated category";
                _response.Data = _mapper.Map<OrderDTO>(_existingOrder);
            }
            catch (Exception ex)
            {
                _response.Status = false;
                _response.Title = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
                _response.Data = null;
            }
            return _response;
        }
    }
}
