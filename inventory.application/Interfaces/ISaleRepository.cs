using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale?> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<Sale> AddAsync(Sale sale);
        Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to);
    }
}
