using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repo;

        public UnitService(IUnitRepository repo)
        {
            _repo = repo;
        }

        public async Task<UnitDto?> GetUnitByIdAsync(int id)
        {
            var unit = await _repo.GetByIdAsync(id);
            return unit is null ? null : Map(unit);
        }

        public async Task<IEnumerable<UnitDto>> GetAllUnitsAsync() =>
            (await _repo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<UnitDto>> GetActiveUnitsAsync() =>
            (await _repo.GetActiveAsync()).Select(Map);

        public async Task<UnitDto> CreateUnitAsync(CreateUnitDto dto)
        {
            var unit = new Unit
            {
                Name = dto.Name,
                Abbreviation = dto.Abbreviation,
                IsActive = true
            };
            await _repo.AddAsync(unit);
            return Map(unit);
        }

        public async Task<UnitDto> UpdateUnitAsync(int id, UpdateUnitDto dto)
        {
            var unit = await _repo.GetByIdAsync(id) ?? throw new ArgumentException("Unit not found");
            unit.Name = dto.Name;
            unit.Abbreviation = dto.Abbreviation;
            unit.IsActive = dto.IsActive;
            await _repo.UpdateAsync(unit);
            return Map(unit);
        }

        public async Task DeactivateUnitAsync(int id)
        {
            var unit = await _repo.GetByIdAsync(id) ?? throw new ArgumentException("Unit not found");
            unit.IsActive = false;
            await _repo.UpdateAsync(unit);
        }

        public async Task ActivateUnitAsync(int id)
        {
            var unit = await _repo.GetByIdAsync(id) ?? throw new ArgumentException("Unit not found");
            unit.IsActive = true;
            await _repo.UpdateAsync(unit);
        }

        public async Task DeleteUnitAsync(int id)
        {
            if (!await _repo.ExistsAsync(id)) throw new ArgumentException("Unit not found");
            await _repo.DeleteAsync(id);
        }

        private static UnitDto Map(Unit u) => new(
            u.Id,
            u.Name,
            u.Abbreviation,
            u.IsActive,
            u.Products?.Count ?? 0);
    }
}
