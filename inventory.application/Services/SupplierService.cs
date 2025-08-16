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
    public class SupplierService(ISupplierRepository repo) : ISupplierService
    {
        public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
        {
            var s = await repo.GetByIdAsync(id);
            return s is null ? null : Map(s);
        }

        public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync() =>
            (await repo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync() =>
            (await repo.GetActiveAsync()).Select(Map);

        public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto)
        {
            var s = new Supplier
            {
                Name = dto.Name,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = true
            };
            await repo.AddAsync(s);
            return Map(s);
        }

        public async Task<SupplierDto> UpdateSupplierAsync(int id, CreateSupplierDto dto)
        {
            var s = await repo.GetByIdAsync(id) ?? throw new ArgumentException("Supplier not found");
            s.Name = dto.Name;
            s.ContactPerson = dto.ContactPerson;
            s.Phone = dto.Phone;
            s.Email = dto.Email;
            s.Address = dto.Address;
            await repo.UpdateAsync(s);
            return Map(s);
        }

        public async Task DeactivateSupplierAsync(int id)
        {
            var s = await repo.GetByIdAsync(id) ?? throw new ArgumentException("Supplier not found");
            s.IsActive = false;
            await repo.UpdateAsync(s);
        }

        public async Task ActivateSupplierAsync(int id)
        {
            var s = await repo.GetByIdAsync(id) ?? throw new ArgumentException("Supplier not found");
            s.IsActive = true;
            await repo.UpdateAsync(s);
        }

        public async Task DeleteSupplierAsync(int id)
        {
            if (!await repo.ExistsAsync(id)) throw new ArgumentException("Supplier not found");
            await repo.DeleteAsync(id);
        }

        private static SupplierDto Map(Supplier s) => new(
            s.Id,
            s.Name,
            s.ContactPerson ?? string.Empty,
            s.Phone ?? string.Empty,
            s.Email ?? string.Empty,
            s.Address ?? string.Empty,
            s.IsActive,
            s.PurchaseOrders?.Count ?? 0,
            s.PurchaseOrders?.Sum(po => po.TotalAmount) ?? 0);
    }
}
