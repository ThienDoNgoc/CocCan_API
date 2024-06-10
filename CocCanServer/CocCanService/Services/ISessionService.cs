using CocCanService.DTOs.OrderDetail;
using CocCanService.DTOs.Session;
using CocCanService.Services.Imp;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface ISessionService
    {
        Task<ServiceResponse<List<SessionDTO>>> GetAllSessionsAsync(string filter);
        Task<ServiceResponse<SessionDTO>> CreateSessionAsync(CreateSessionDTO createSessionDTO);
        Task<ServiceResponse<SessionDTO>> UpdateSessionAsync(SessionDTO SessionDTO);
        Task<ServiceResponse<string>> SoftDeleteSessionAsync(Guid id);
        Task<ServiceResponse<bool>> HardDeleteSessionAsync(SessionDTO SessionDTO);
        Task<ServiceResponse<SessionDTO>> GetSessionByGUIDAsync(Guid id);
    }
}
