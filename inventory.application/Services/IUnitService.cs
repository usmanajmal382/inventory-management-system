using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IUnitService
    {
        Task<UnitDto?> GetUnitByIdAsync(int id);
        Task<IEnumerable<UnitDto>> GetAllUnitsAsync();
        Task<IEnumerable<UnitDto>> GetActiveUnitsAsync();
        Task<UnitDto> CreateUnitAsync(CreateUnitDto dto);
        Task<UnitDto> UpdateUnitAsync(int id, UpdateUnitDto dto);
        Task DeactivateUnitAsync(int id);
        Task ActivateUnitAsync(int id);
        Task DeleteUnitAsync(int id);
    }
}
