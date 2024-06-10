using AutoMapper;
using CocCanService.DTOs.Menu;
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
    public class MenuService: IMenuService
    {
        private readonly IMenuRepository _menuRepo;
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository menuRepo, IMapper mapper)
        {
            this._menuRepo = menuRepo;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<MenuDTO>> CreateMenuAsync(CreateMenuDTO createMenuDTO)
        {
            ServiceResponse<MenuDTO> _response = new();
            try
            {
                var _newMenu = _mapper.Map<Menu>(createMenuDTO);

                if (!await _menuRepo.CreateMenuAsync(_newMenu))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Category Repository when trying to create store!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Created";
                _response.Data = _mapper.Map<MenuDTO>(_newMenu);
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

        public async Task<ServiceResponse<List<MenuDTO>>> GetAllMenusAsync()
        {
            ServiceResponse<List<MenuDTO>> _response = new();
            try
            {
                var _MenuList = await _menuRepo.GetAllMenusWithStatusAsync();

                var _MenuListDTO = new List<MenuDTO>();

                foreach (var item in _MenuList)
                {
                    _MenuListDTO.Add(_mapper.Map<MenuDTO>(item));
                }

                _response.Status = true;
                _response.Title = "Got all TimeSlots";
                _response.Data = _MenuListDTO;
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

        public async Task<ServiceResponse<MenuDTO>> GetMenuByIdAsync(Guid id)
        {
            ServiceResponse<MenuDTO> _response = new();
            try
            {
                var _menuList = await _menuRepo.GetMenuByGUIDAsync(id);

                if (_menuList == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                var _menuDto = _mapper.Map<MenuDTO>(_menuList);

                _response.Status = true;
                _response.Title = "Got TimeSlot";
                _response.Data = _menuDto;

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

        public async Task<ServiceResponse<string>> SoftDeleteMenuAsync(Guid id)
        {
            ServiceResponse<string> _response = new();
            try
            {
                var _existingMenu = await _menuRepo.GetMenuByGUIDAsync(id);

                if (_existingMenu == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                if (!await _menuRepo.SoftDeleteMenuAsync(id))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to delete category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Deleted TimeSlot";
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

        public async Task<ServiceResponse<MenuDTO>> UpdateMenuAsync(Guid id, UpdateMenuDTO updateMenuDTO)
        {
            ServiceResponse<MenuDTO> _response = new();
            try
            {
                var _existingMenu = await _menuRepo.GetMenuByGUIDAsync(id);

                if (_existingMenu == null)
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Not Found!");
                    _response.Data = null;
                    return _response;
                }

                _existingMenu = _mapper.Map<UpdateMenuDTO, Menu>(updateMenuDTO, _existingMenu);

                if (!await _menuRepo.UpdateMenuAsync(_existingMenu))
                {
                    _response.Status = false;
                    _response.Title = "Error";
                    _response.ErrorMessages.Add("Some error occur in Store Repository when trying to update category!");
                    _response.Data = null;
                    return _response;
                }

                _response.Status = true;
                _response.Title = "Updated category";
                _response.Data = _mapper.Map<MenuDTO>(_existingMenu);
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
