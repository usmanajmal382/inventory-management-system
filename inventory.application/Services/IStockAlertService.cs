using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IStockAlertService
    {
        Task<IEnumerable<StockAlertDto>> GetAllAlertsAsync();
        Task<IEnumerable<StockAlertDto>> GetUnreadAlertsAsync();
        Task<IEnumerable<StockAlertDto>> GetAlertsByProductIdAsync(int productId);
        Task MarkAlertAsReadAsync(int id);
        Task MarkAllAlertsAsReadAsync();
        Task GenerateLowStockAlertsAsync();
        Task DeleteAlertAsync(int id);
    }
}
