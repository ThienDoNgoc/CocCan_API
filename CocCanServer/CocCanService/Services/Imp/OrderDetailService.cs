using AutoMapper;
using CocCanService.DTOs.Menu;
using CocCanService.DTOs.Order;
using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.Product;
using CocCanService.DTOs.Store;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services.Imp
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepo, IStoreRepository storeRepo, IMapper mapper)
        {
            this._orderDetailRepo = orderDetailRepo;
            this._storeRepo = storeRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<OrderDetailDTO>> CreateOrderDetailAsync(CreateOrderDetailDTO createOrderDetailDTO, Guid id)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var _newOrderDetail = _mapper.Map<OrderDetail>(createOrderDetailDTO);
                _newOrderDetail.OrderId = id;
                if (!await _orderDetailRepo.CreateOrderDetailAsync(_newOrderDetail))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Category Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }


                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<OrderDetailDTO>(_newOrderDetail);

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

        public async Task<ServiceResponse<List<OrderDetailDTO>>> GetAllOrderDetailsAsync(string filter, string range, string sort)
        {
            ServiceResponse<List<OrderDetailDTO>> _response = new();
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

                var _OrderDetailList = await _orderDetailRepo
                    .GetAllOrderDetailsAsync(
                        _filter, _range[0] + 1, _range[1] + 1, _sort[0], (_sort[1] == "ASC")
                    );
                var _OrderDetailListDTO = new List<OrderDetailDTO>();

                foreach (var item in _OrderDetailList)
                {
                    _OrderDetailListDTO.Add(_mapper.Map<OrderDetailDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all stores";
                _response.Data = _OrderDetailListDTO;
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

        public async Task<ServiceResponse<List<OrderDetailDTO>>> GetAllOrderDetailsByOrderIDAsync(Guid orderID)
        {
            ServiceResponse<List<OrderDetailDTO>> _response = new();
            try
            {
                var _OrderDetailList = await _orderDetailRepo.GetAllOrderDetailsByOrderIDAsync(orderID);

                var _OderDetailListDTO = new List<OrderDetailDTO>();

                foreach (var item in _OrderDetailList)
                {
                    _OderDetailListDTO.Add(_mapper.Map<OrderDetailDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all categories";
                _response.Data = _OderDetailListDTO;
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

        public async Task<ServiceResponse<OrderDetailDTO>> GetOrderDetailByIdAsync(Guid id)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var _orderDetailList = await _orderDetailRepo.GetOrderDetailByGUIDAsync(id);

                if (_orderDetailList == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _orderDetailDto = _mapper.Map<OrderDetailDTO>(_orderDetailList);

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _orderDetailDto;

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

        public async Task<ServiceResponse<List<OrderDetailBatchDTO>>> GetOrderDetailByBatch(Guid id)
        {
            ServiceResponse<List<OrderDetailBatchDTO>> _response = new();
            try
            {
                Dictionary<string, List<string>> _filter = System.Text.Json.JsonSerializer
                            .Deserialize<Dictionary<string, List<string>>>("{\"session\":[\""+id.ToString()+"\"]}");
                var _stores = await _storeRepo
                    .GetAllStoresWithStatusAsync(
                        _filter, 0, 0, "", true
                    );

                List<OrderDetailBatchDTO> _orderDetailBatchDTOs = new List<OrderDetailBatchDTO>();

                foreach (var item in _stores)
                {
                    var _orderDetailList = await _orderDetailRepo.GetOrderDetailByBatch(id, item.Id);

                    var _orderDetailListDTO = new List<OrderDetailDTO>();

                    foreach (var item2 in _orderDetailList)
                    {
                        _orderDetailListDTO.Add(_mapper.Map<OrderDetailDTO>(item2));
                        _orderDetailListDTO[_orderDetailListDTO.Count - 1].Product = _mapper.Map<ProductDTO>(item2.MenuDetail.Product);
                    }

                    OrderDetailBatchDTO orderDetailBatchDTO = new OrderDetailBatchDTO()
                    {
                        storeDTO = _mapper.Map<StoreDTO>(item),
                        orderDetailDTOs = _orderDetailListDTO
                    };

                    _orderDetailBatchDTOs.Add(orderDetailBatchDTO);
                }

                if (_orderDetailBatchDTOs == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _orderDetailBatchDTOs;

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

        public async Task<ServiceResponse<string>> SoftDeleteOrderDetailAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingOrderDetail = await _orderDetailRepo.GetOrderDetailByGUIDAsync(id);

                if (_existingOrderDetail == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _orderDetailRepo.SoftDeleteOrderDetailAsync(id))
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

        public async Task<ServiceResponse<OrderDetailDTO>> UpdateOrderDetailAsync(Guid id, UpdateOrderDetailDTO updateorderDetailDTO)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var _existingOrderDetail = await _orderDetailRepo.GetOrderDetailByGUIDAsync(id);

                if (_existingOrderDetail == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingOrderDetail = _mapper.Map<UpdateOrderDetailDTO, OrderDetail>(updateorderDetailDTO, _existingOrderDetail);

                if (!await _orderDetailRepo.UpdateOrderDetailAsync(_existingOrderDetail))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated category";
                _response.Data = _mapper.Map<OrderDetailDTO>(_existingOrderDetail);
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
