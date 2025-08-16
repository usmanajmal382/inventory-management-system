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

    public class StockAlertService(
        IStockAlertRepository alertRepo,
        IProductRepository prodRepo) : IStockAlertService
    {
        public async Task<IEnumerable<StockAlertDto>> GetAllAlertsAsync() =>
            (await alertRepo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<StockAlertDto>> GetUnreadAlertsAsync() =>
            (await alertRepo.GetUnreadAsync()).Select(Map);

        public async Task<IEnumerable<StockAlertDto>> GetAlertsByProductIdAsync(int productId) =>
            (await alertRepo.GetByProductIdAsync(productId)).Select(Map);

        public async Task MarkAlertAsReadAsync(int id) => await alertRepo.MarkAsReadAsync(id);
        public async Task MarkAllAlertsAsReadAsync() => await alertRepo.MarkAllAsReadAsync();

        public async Task GenerateLowStockAlertsAsync()
        {
            var lowStock = await prodRepo.GetLowStockProductsAsync();
            foreach (var p in lowStock)
            {
                var unread = (await alertRepo.GetByProductIdAsync(p.Id))
                    .Any(a => !a.IsRead && (a.Type == StockAlertType.LowStock || a.Type == StockAlertType.OutOfStock));
                if (unread) continue;

                var alert = new StockAlert
                {
                    ProductId = p.Id,
                    Type = p.Quantity == 0 ? StockAlertType.OutOfStock : StockAlertType.LowStock,
                    Message = p.Quantity == 0
                        ? $"Product '{p.Name}' is out of stock."
                        : $"Product '{p.Name}' is low in stock. Current: {p.Quantity}, Min: {p.MinStockLevel}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };
                await alertRepo.AddAsync(alert);
            }
        }

        public async Task DeleteAlertAsync(int id) => await alertRepo.DeleteAsync(id);

        private static StockAlertDto Map(StockAlert a) => new(
            a.Id, a.ProductId, a.Product?.Name, a.Product?.SKU, a.Type, a.Message, a.IsRead, a.CreatedAt);
    }
}
