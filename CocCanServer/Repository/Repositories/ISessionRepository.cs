using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.repositories
{
    public interface ISessionRepository
    {
        Task<ICollection<Session>>
            GetAllSessionsWithStatusAsync(Dictionary<string, List<string>> filter);
        Task<bool> CreateSessionAsync(Session session);
        Task<bool> UpdateSessionAsync(Session session);
        Task<bool> SoftDeleteSessionAsync(Guid id);
        Task<Session> GetSessionByGUIDAsync(Guid id);
    }
}
