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

    public class StockTransactionRepository(AppDbContext ctx) : IStockTransactionRepository
    {
        public async Task<StockTransaction?> GetByIdAsync(int id) =>
            await ctx.StockTransactions.Include(t => t.Product).FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<StockTransaction>> GetByProductIdAsync(int productId) =>
            await ctx.StockTransactions.Include(t => t.Product).Where(t => t.ProductId == productId).OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task<IEnumerable<StockTransaction>> GetAllAsync() =>
            await ctx.StockTransactions.Include(t => t.Product).OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task<StockTransaction> AddAsync(StockTransaction transaction)
        {
            ctx.StockTransactions.Add(transaction);
            await ctx.SaveChangesAsync();
            return transaction;
        }
    }
}
