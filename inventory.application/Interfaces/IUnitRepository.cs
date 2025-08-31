using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Interfaces
{
    public interface IUnitRepository
    {
        Task<Unit?> GetByIdAsync(int id);
        Task<IEnumerable<Unit>> GetAllAsync();
        Task<IEnumerable<Unit>> GetActiveAsync();
        Task<Unit> AddAsync(Unit unit);
        Task UpdateAsync(Unit unit);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
