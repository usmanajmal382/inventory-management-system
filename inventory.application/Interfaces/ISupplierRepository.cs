using inventory.core.Entities;

namespace inventory.application.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier?> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<IEnumerable<Supplier>> GetActiveAsync();
        Task<Supplier> AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

}
