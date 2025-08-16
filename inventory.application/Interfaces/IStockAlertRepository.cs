using inventory.core.Entities;

namespace inventory.application.Interfaces
{
    public interface IStockAlertRepository
    {
        Task<StockAlert?> GetByIdAsync(int id);
        Task<IEnumerable<StockAlert>> GetAllAsync();
        Task<IEnumerable<StockAlert>> GetUnreadAsync();
        Task<IEnumerable<StockAlert>> GetByProductIdAsync(int productId);
        Task<StockAlert> AddAsync(StockAlert alert);
        Task UpdateAsync(StockAlert alert);
        Task DeleteAsync(int id);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync();
    }

}
