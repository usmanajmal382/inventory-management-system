using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IStockService
    {
        Task<StockTransactionDto> CreateStockTransactionAsync(CreateStockTransactionDto dto);
        Task<IEnumerable<StockTransactionDto>> GetStockTransactionsByProductIdAsync(int productId);
        Task<IEnumerable<StockTransactionDto>> GetAllStockTransactionsAsync();
    }
}
