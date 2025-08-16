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
    public class SupplierRepository(AppDbContext ctx) : ISupplierRepository
    {
        public async Task<Supplier?> GetByIdAsync(int id) =>
            await ctx.Suppliers.Include(s => s.PurchaseOrders).FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<Supplier>> GetAllAsync() =>
            await ctx.Suppliers.Include(s => s.PurchaseOrders).OrderBy(s => s.Name).ToListAsync();

        public async Task<IEnumerable<Supplier>> GetActiveAsync() =>
            await ctx.Suppliers.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            ctx.Suppliers.Add(supplier);
            await ctx.SaveChangesAsync();
            return supplier;
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            ctx.Suppliers.Update(supplier);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var s = await ctx.Suppliers.FindAsync(id);
            if (s is not null) { ctx.Suppliers.Remove(s); await ctx.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.Suppliers.AnyAsync(s => s.Id == id);
    }

}
