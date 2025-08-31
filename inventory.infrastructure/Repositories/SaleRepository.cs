using inventory.application.Interfaces;
using inventory.core.Entities;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _ctx;

        public SaleRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Sale?> GetByIdAsync(int id) =>
            await _ctx.Sales
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<Sale>> GetAllAsync() =>
            await _ctx.Sales
                .Include(s => s.Items)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime from, DateTime to) =>
            await _ctx.Sales
                .Where(s => s.SaleDate >= from && s.SaleDate <= to)
                .Include(s => s.Items)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();

        public async Task<Sale> AddAsync(Sale sale)
        {
            _ctx.Sales.Add(sale);
            await _ctx.SaveChangesAsync();
            return sale;
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to) =>
            await _ctx.Sales
                .Where(s => s.SaleDate >= from && s.SaleDate <= to)
                .SumAsync(s => s.TotalAmount);
    }
}
