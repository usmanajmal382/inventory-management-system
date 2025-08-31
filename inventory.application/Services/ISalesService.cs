using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface ISalesService
    {
        Task<SaleDto?> GetSaleByIdAsync(int id);
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
        Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime from, DateTime to);
        Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime date);
        Task<SaleDto> CreateSaleFromOrderAsync(int orderId);
    }
}
