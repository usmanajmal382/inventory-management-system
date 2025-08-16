using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface ISupplierService
    {
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<IEnumerable<SupplierDto>> GetActiveSuppliersAsync();
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        Task<SupplierDto> UpdateSupplierAsync(int id, CreateSupplierDto dto);
        Task DeactivateSupplierAsync(int id);
        Task ActivateSupplierAsync(int id);
        Task DeleteSupplierAsync(int id);
    }
}
