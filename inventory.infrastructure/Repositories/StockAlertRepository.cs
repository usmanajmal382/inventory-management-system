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
    public class StockAlertRepository(AppDbContext ctx) : IStockAlertRepository
    {
        public async Task<StockAlert?> GetByIdAsync(int id) =>
            await ctx.StockAlerts.Include(a => a.Product).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<StockAlert>> GetAllAsync() =>
            await ctx.StockAlerts.Include(a => a.Product).OrderByDescending(a => a.CreatedAt).ToListAsync();

        public async Task<IEnumerable<StockAlert>> GetUnreadAsync() =>
            await ctx.StockAlerts.Include(a => a.Product).Where(a => !a.IsRead).OrderByDescending(a => a.CreatedAt).ToListAsync();

        public async Task<IEnumerable<StockAlert>> GetByProductIdAsync(int productId) =>
            await ctx.StockAlerts.Include(a => a.Product).Where(a => a.ProductId == productId).OrderByDescending(a => a.CreatedAt).ToListAsync();

        public async Task<StockAlert> AddAsync(StockAlert alert)
        {
            ctx.StockAlerts.Add(alert);
            await ctx.SaveChangesAsync();
            return alert;
        }

        public async Task UpdateAsync(StockAlert alert)
        {
            ctx.StockAlerts.Update(alert);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await ctx.StockAlerts.FindAsync(id);
            if (a is not null) { ctx.StockAlerts.Remove(a); await ctx.SaveChangesAsync(); }
        }

        public async Task MarkAsReadAsync(int id)
        {
            var a = await ctx.StockAlerts.FindAsync(id);
            if (a is not null) { a.IsRead = true; await UpdateAsync(a); }
        }

        public async Task MarkAllAsReadAsync()
        {
            var unread = await ctx.StockAlerts.Where(a => !a.IsRead).ToListAsync();
            unread.ForEach(a => a.IsRead = true);
            await ctx.SaveChangesAsync();
        }
    }
}
