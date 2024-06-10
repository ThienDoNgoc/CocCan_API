using CocCanService.DTOs.Menu;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IMenuService
    {
        Task<ServiceResponse<List<DTOs.Menu.MenuDTO>>> GetAllMenusAsync();
        Task<ServiceResponse<DTOs.Menu.MenuDTO>> CreateMenuAsync(CreateMenuDTO createMenuDTO);
        Task<ServiceResponse<DTOs.Menu.MenuDTO>> UpdateMenuAsync(Guid id, UpdateMenuDTO updateMenuDTO);
        Task<ServiceResponse<DTOs.Menu.MenuDTO>> GetMenuByIdAsync(Guid id);
        Task<ServiceResponse<string>> SoftDeleteMenuAsync(Guid id);
    }
}
