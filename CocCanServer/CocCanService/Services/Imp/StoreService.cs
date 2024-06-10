using AutoMapper;
using CocCanService.DTOs.Staff;
using CocCanService.DTOs.Store;
using Newtonsoft.Json.Linq;
using Repository.Entities;
using Repository.repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CocCanService.Services.Imp
{
    public class StoreService:IStoreService
    {
        private readonly IStoreRepository _storeRepo;
        private readonly IMapper _mapper;

        public StoreService(IStoreRepository storeRepo, IMapper mapper)
        {
            this._storeRepo = storeRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<StoreDTO>> CreateStoreAsync(CreateStoreDTO createStoreDTO)
        {
            ServiceResponse<StoreDTO> _response = new ServiceResponse<StoreDTO>();
            try
            {
                var _newStore = _mapper.Map<Store>(createStoreDTO);

                if (!await _storeRepo.CreateStoreAsync(_newStore))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<StoreDTO>(_newStore);
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

        public async Task<ServiceResponse<List<StoreDTO>>> 
            GetAllStoresWithStatusAsync(string filter, string range, string sort)
        {
            ServiceResponse<List<StoreDTO>> _response = new ServiceResponse<List<StoreDTO>>();
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
                    _range = new List<int>() { -1,-1};
                if (sort != null)
                    _sort = System.Text.Json.JsonSerializer.Deserialize<List<string>>(sort);
                else
                    _sort = new List<string>() { "default", "" };

                var _StoreList = await _storeRepo
                    .GetAllStoresWithStatusAsync(
                        _filter, _range[0] + 1, _range[1] + 1, _sort[0], (_sort[1] == "ASC")
                    );
                if (_filter != null && _filter.ContainsKey("search"))
                {
                    _StoreList = _StoreList.Where(s => _filter["search"].Any(f => s.Name.ToLower().Contains(f.ToLower()))).ToList();
                    //if(_StoreList == null)
                    //    _StoreList =  _StoreList.Where(s => s.Products.Any(p => _filter["search"].Any(f => p.Name.ToLower().Contains(f.ToLower())))).ToList();
                }
                    

                var _StoreListDTO = new List<StoreDTO>();   

                foreach (var item in _StoreList)
                {
                    _StoreListDTO.Add(_mapper.Map<StoreDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all stores";
                _response.Data = _StoreListDTO;
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

        public async Task<ServiceResponse<StoreDTO>> GetStoreByIdAsync(Guid id)
        {
            ServiceResponse<StoreDTO> _response = new ServiceResponse<StoreDTO>();
            try
            {
                Dictionary<string, string> _filter = null;

                var _StoreList = await _storeRepo.GetStoreByGUIDAsync(id);

                if (_StoreList == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _StoreDto = _mapper.Map<StoreDTO>(_StoreList);

                _response.Status = true;
                _response.Title = "Got store";
                _response.Data = _StoreDto;

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

        public async Task<ServiceResponse<string>> SoftDeleteStoreAsync(Guid id)
        {
            ServiceResponse<string> _response = new ServiceResponse<string>();
            try
            {
                var _existingStore = await _storeRepo.GetStoreByGUIDAsync(id);
                
                if (_existingStore == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _storeRepo.SoftDeleteStoreAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted store";
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

        public async Task<ServiceResponse<StoreDTO>> UpdateStoreAsync(Guid id, UpdateStoreDTO updateStoreDTO)
        {
            ServiceResponse<StoreDTO> _response = new ServiceResponse<StoreDTO>();
            try
            {
                var _existingStore = await _storeRepo.GetStoreByGUIDAsync(id);

                if (_existingStore == null)
                {
                    _response.Status = false;
                    _response.Title = "NotFound";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingStore = _mapper.Map<UpdateStoreDTO, Store>(updateStoreDTO, _existingStore);

                if (!await _storeRepo.UpdateStoreAsync(_existingStore))
                {
                    _response.Status = false;
                    _response.Title = "RepoError";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated store";
                _response.Data = _mapper.Map<StoreDTO>(_existingStore);
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
