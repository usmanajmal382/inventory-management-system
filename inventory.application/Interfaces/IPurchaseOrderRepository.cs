using inventory.core.Entities;

namespace inventory.application.Interfaces
{
    public interface IPurchaseOrderRepository
    {
        Task<PurchaseOrder?> GetByIdAsync(int id);
        Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<PurchaseOrder>> GetAllAsync();
        Task<IEnumerable<PurchaseOrder>> GetBySupplierAsync(int supplierId);
        Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(PurchaseOrderStatus status);
        Task<PurchaseOrder> AddAsync(PurchaseOrder purchaseOrder);
        Task UpdateAsync(PurchaseOrder purchaseOrder);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<string> GenerateOrderNumberAsync();
    }

}
