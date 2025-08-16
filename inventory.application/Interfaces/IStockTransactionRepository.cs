using inventory.core.Entities;

namespace inventory.application.Interfaces
{
    public interface IStockTransactionRepository
    {
        Task<StockTransaction?> GetByIdAsync(int id);
        Task<IEnumerable<StockTransaction>> GetByProductIdAsync(int productId);
        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task<StockTransaction> AddAsync(StockTransaction transaction);
    }
}
